/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net is licensed under the LGPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using bedrock.util;

using System.Security.Authentication;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

namespace bedrock.net
{
    /// <summary>
    /// Delegate for members that receive a socket.
    /// </summary>
    public delegate void AsyncSocketHandler(object sender, BaseSocket sock);

    /// <summary>
    /// An asynchronous socket, which calls a listener class when
    /// interesting things happen.
    /// </summary>
    [SVN(@"$Id: AsyncSocket.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class AsyncSocket : BaseSocket, IComparable
    {
        /// <summary>
        /// Socket states.
        /// </summary>
        [SVN(@"$Id: AsyncSocket.cs 724 2008-08-06 18:09:25Z hildjj $")]
            private enum SocketState
        {
            /// <summary>
            /// Socket has been created.
            /// </summary>
            Created,
            /// <summary>
            /// Socket is listening for new connections
            /// </summary>
            Listening,
            /// <summary>
            /// Doing DNS lookup
            /// </summary>
            Resolving,
            /// <summary>
            /// Attempting to connect
            /// </summary>
            Connecting,
            /// <summary>
            /// Connected to a peer.  The running state.
            /// </summary>
            Connected,
            /// <summary>
            /// Shutting down the socket.
            /// </summary>
            Closing,
            /// <summary>
            /// Closed down.
            /// </summary>
            Closed,
            /// <summary>
            /// An error ocurred.
            /// </summary>
            Error
        }

        private const int BUFSIZE = 4096;

        /// <summary> The set of allowable errors in SSL certificates
        /// if UntrustedRootOK is set to true.  </summary>
        [Obsolete("Catch OnInvalidCertificate, instead")]
        public const SslPolicyErrors DefaultUntrustedPolicy =
                 SslPolicyErrors.RemoteCertificateChainErrors;

        /// <summary> The allowable SSL certificate errors.  If you
        /// modify UntrustedRootOK to true, the side effect will be to
        /// set this to DefaultUntrustedPolicy.  False, the default,
        /// sets this to None.  </summary>
        private static SslPolicyErrors AllowedSSLErrors = SslPolicyErrors.None;

        /// <summary>
        /// Are untrusted root certificates OK when connecting using
        /// SSL?  Setting this to true is insecure, but it's unlikely
        /// that you trust jabbber.org or jabber.com's relatively
        /// bogus certificate roots.
        ///
        /// Setting this modifies AllowedSSLErrors by side-effect.
        /// </summary>
        [DefaultValue(false)]
        [Obsolete("Catch OnInvalidCertificate, instead")]
        public static bool UntrustedRootOK
        {
            get
            {
                return (AllowedSSLErrors != SslPolicyErrors.None);
            }
            set
            {
                if (value)
                {
                    AllowedSSLErrors = DefaultUntrustedPolicy;
                }
                else
                {
                    AllowedSSLErrors = SslPolicyErrors.None;
                }
            }
        }

        /// <summary>
        /// The types of SSL to support.  SSL3 and TLS1 by default.
        /// That should be good enough for most apps, and was
        /// hard-coded to start with.  Note: when doing start-tls,
        /// this is overridden to just be TLS.
        /// </summary>
        public static SslProtocols   SSLProtocols        = SslProtocols.Ssl3 | SslProtocols.Tls;
        private SslProtocols         m_secureProtocol    = SslProtocols.None;
        private Socket               m_sock              = null;
        private X509Certificate2     m_cert              = null;
        private Stream               m_stream            = null;
        private SslStream            m_sslStream         = null;  // hold on to the SSL stream as it goes by, since compression might happen later.
        private MemoryStream         m_pending           = new MemoryStream();
        private bool                 m_writing           = false;
        private bool                 m_requireClientCert = false;
        private bool                 m_cert_gui          = true;
        private bool                 m_server            = false;
        private byte[]               m_buf               = new byte[BUFSIZE];
        private SocketState          m_state             = SocketState.Created;
        private SocketWatcher        m_watcher           = null;
        private Guid                 m_id                = Guid.NewGuid();
        private bool                 m_reading           = false;
        private bool                 m_synch             = false;
        private Address              m_addr;


        /// <summary>
        /// Called from SocketWatcher.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="listener">The listener for this socket</param>
        public AsyncSocket(SocketWatcher w, ISocketEventListener listener) : base(listener)
        {
            m_watcher = w;
        }

        /// <summary>
        /// Called from SocketWatcher.
        /// </summary>
        /// <param name="w"></param>
        /// <param name="listener">The listener for this socket</param>
        /// <param name="SSL">Do SSL3 and TLS1 on startup (call
        /// StartTLS later if this is false, and TLS only is needed
        /// later)</param>
        /// <param name="synch">Synchronous operation</param>
        public AsyncSocket(SocketWatcher w,
                           ISocketEventListener listener,
                           bool SSL,
                           bool synch) :
            base(listener)
        {
            m_watcher = w;
            m_synch = synch;

            if (SSL)
                m_secureProtocol = SSLProtocols;
        }

        private AsyncSocket(SocketWatcher w) : base()
        {
            m_watcher = w;
        }

        /*
        /// <summary>
        /// Return the state of the socket.  WARNING: don't use this.
        /// </summary>
        public State Socket_State
        {
            get
            {
                return m_state;
            }
        }
        */
        private SocketState State
        {
            get { return m_state; }
            set
            {
// useful for finding unexpected socket closes.
//                Debug.WriteLine("socket state: " + m_state.ToString() + "->" + value.ToString());
                m_state = value;
            }
        }

        /// <summary>
        /// For connect sockets, the remote address.  For Accept sockets, the local address.
        /// </summary>
        public Address Address
        {
            get
            {
                return m_addr;
            }
        }

        /// <summary>
        /// Get the certificate of the remote endpoint of the socket.
        /// </summary>
        public X509Certificate RemoteCertificate
        {
            get
            {
                if (m_sslStream == null)
                    return null;
                return m_sslStream.RemoteCertificate;
            }
        }

        /// <summary>
        /// Choose a certificate from the local store.  If there are
        /// none available, returns right away.
        /// If there is exactly one, uses it.
        /// Otherwise, prompts.
        /// </summary>
        [Obsolete("Pass in a list of acceptable issuers")]
        public void ChooseClientCertificate()
        {
            ChooseClientCertificate(null);
        }

        /// <summary>
        /// Choose a certificate from the local store.  If there are
        /// none available, returns right away.
        /// If there is exactly one, uses it.
        /// Otherwise, prompts.
        /// TODO: figure out something for server certs, too.
            /// </summary>
        /// <param name="acceptableIssuers">A list of DNs of CAs that are trusted by the other party</param>
        public void ChooseClientCertificate(string[] acceptableIssuers)
        {
            X509Store store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection coll = new X509Certificate2Collection();
            if (acceptableIssuers == null)
            {
                coll.AddRange(store.Certificates);
            }
            else
            {
                foreach (X509Certificate2 cert in store.Certificates)
                {
                    foreach (string issuer in acceptableIssuers)
                    {
                        if (cert.Issuer == issuer)
                        {
                            coll.Add(cert);
                        }
                    }
                }
            }

            switch (coll.Count)
            {
                case 0:
                    return;
                case 1:
                    m_cert = coll[0];
                    return;
                default:
                    #if __MonoCS__
                        m_cert = null;
                    #else
                    X509Certificate2Collection certs = X509Certificate2UI.SelectFromCollection(
                        coll,
                        "Select certificate",
                        "Use this certificate to log in",
                        X509SelectionFlag.SingleSelection);
                    if (certs.Count > 0)
                        m_cert = certs[0];
                    #endif
                    break;
            }
        }

        /// <summary>
        /// Callback to choose client cert.
        /// TODO: this should surface an event of some kind.
        /// </summary>
        public X509Certificate ChooseClientCertificate(Object sender,
            string targetHost,
            X509CertificateCollection localCertificates,
            X509Certificate remoteCertificate,
            string[] acceptableIssuers)
        {
            // this will be called twice if the server requires a client cert.
            // Ignore the callback the first time; I think this is a .Net bug.
            if (acceptableIssuers.Length == 0)
                return m_cert;

            if (CertificateGui)
            {
                if (m_cert != null)
                    return m_cert;

                ChooseClientCertificate(acceptableIssuers);
            }
            return m_cert;
        }

        /// <summary>
        /// If true the certificate selection dialog is called.
        /// </summary>
        public bool CertificateGui
        {
            get { return m_cert_gui; }
            set { m_cert_gui = value; }
        }

        /// <summary>
        /// The local certificate of the socket.
        /// </summary>
        public X509Certificate2 LocalCertificate
        {
            get { return m_cert; }
            set { m_cert = value; }
        }

        /// <summary>
        /// Are we using SSL/TLS?
        /// </summary>
        public bool SSL
        {
            get
            {
                if (m_sslStream == null)
                    return false;
                return m_sslStream.IsEncrypted;
            }
        }

        /// <summary>
        /// Is the socket connected?
        /// </summary>
        public override bool Connected
        {
            get
            {
                if (m_sock == null)
                    return false;
                return m_sock.Connected;
            }
        }

        /// <summary>
        /// Sets the specified option to the specified value.
        /// </summary>
        /// <param name="optionLevel"></param>
        /// <param name="optionName"></param>
        /// <param name="optionValue"></param>
        public void SetSocketOption(SocketOptionLevel optionLevel,
            SocketOptionName optionName,
            byte[] optionValue)
        {
            m_sock.SetSocketOption(optionLevel, optionName, optionValue);
        }

        /// <summary>
        /// Sets the specified option to the specified value.
        /// </summary>
        /// <param name="optionLevel"></param>
        /// <param name="optionName"></param>
        /// <param name="optionValue"></param>
        public void SetSocketOption(SocketOptionLevel optionLevel,
            SocketOptionName optionName,
            int optionValue)
        {
            m_sock.SetSocketOption(optionLevel, optionName, optionValue);
        }

        /// <summary>
        /// Sets the specified option to the specified value.
        /// </summary>
        /// <param name="optionLevel"></param>
        /// <param name="optionName"></param>
        /// <param name="optionValue"></param>
        public void SetSocketOption(SocketOptionLevel optionLevel,
            SocketOptionName optionName,
            object optionValue)
        {
            m_sock.SetSocketOption(optionLevel, optionName, optionValue);
        }

        /// <summary>
        /// Prepare to start accepting inbound requests.  Call
        /// RequestAccept() to start the async process.
        /// </summary>
        /// <param name="addr">Address to listen on</param>
        /// <param name="backlog">The Maximum length of the queue of
        /// pending connections</param>
        public override void Accept(Address addr, int backlog)
        {
            lock (this)
            {
                m_addr = addr;

                m_sock = new Socket(AddressFamily.InterNetwork,
                                    SocketType.Stream,
                                    ProtocolType.Tcp);

                // Always reuse address.
                m_sock.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, 1);
                m_sock.Bind(m_addr.Endpoint);
                m_sock.Listen(backlog);
                State = SocketState.Listening;

                if (m_watcher != null)
                    m_watcher.RegisterSocket(this);
            }
        }

        /// <summary>
        /// Start the flow of async accepts.  Flow will continue while
        /// Listener.OnAccept() returns true.  Otherwise, call
        /// RequestAccept() again to continue.
        /// </summary>
        public override void RequestAccept()
        {
            lock (this)
            {
                if (State != SocketState.Listening)
                {
                    throw new InvalidOperationException("Not a listen socket");
                }
            }
            if (m_synch)
            {
                Socket cli;
                try
                {
                    cli = m_sock.Accept();
                }
                catch (SocketException)
                {
                    Debug.WriteLine("A cancel call was sent to the accepting socket.");
                    return;
                }

                AsyncSocket cliCon = new AsyncSocket(m_watcher);
                cliCon.m_sock = cli;
                cliCon.m_synch = true;
                AcceptDone(cliCon);
            }
            else
            {
                m_sock.BeginAccept(new AsyncCallback(ExecuteAccept), null);
            }
        }

        /// <summary>
        /// We got a connection from outside.  Add it to the SocketWatcher.
        /// </summary>
        /// <param name="ar"></param>
        private void ExecuteAccept(IAsyncResult ar)
        {
            Socket cli = (Socket) m_sock.EndAccept(ar);
            AsyncSocket cliCon = new AsyncSocket(m_watcher);
            cliCon.m_sock = cli;
            AcceptDone(cliCon);
        }

        private void AcceptDone(AsyncSocket cliCon)
        {
            cliCon.m_addr = m_addr;
            cliCon.Address.IP = ((IPEndPoint) cliCon.m_sock.RemoteEndPoint).Address;
            cliCon.State = SocketState.Connected;

            cliCon.m_stream = new NetworkStream(cliCon.m_sock);
            cliCon.m_server = true;
            cliCon.LocalCertificate = m_cert;
            cliCon.RequireClientCert = m_requireClientCert;

            ISocketEventListener l = m_listener.GetListener(cliCon);
            if (l == null)
            {
                // if the listener returns null, close the socket and
                // quit, instead of asserting.
                cliCon.m_sock.Close();
                RequestAccept();
                return;
            }

            cliCon.m_listener = l;

            try
            {
                if (m_watcher != null)
                    m_watcher.RegisterSocket(cliCon);
            }
            catch (InvalidOperationException)
            {
                // m_watcher out of slots.
                cliCon.AsyncClose();

                // don't set state
                // they really don't need this error, we don't think.
                // Error(e);

                // tell the watcher that when it gets its act together,
                // we'd appreciate it if it would restart the RequestAccept().
                m_watcher.PendingAccept(this);
                return;
            }

            if (m_secureProtocol != SslProtocols.None)
                cliCon.StartTLS();

            if (l.OnAccept(cliCon))
            {
                RequestAccept();
            }
        }

        /// <summary>
        /// Outbound connection.  Eventually calls Listener.OnConnect() when
        /// the connection comes up.  Don't forget to call RequestRead() in
        /// OnConnect()!
        /// </summary>
        /// <param name="addr"></param>
        public override void Connect(Address addr)
        {
            // Debug.WriteLine("starting connect to " + addr.ToString());
            State = SocketState.Resolving;
            if (m_synch)
            {
                addr.Resolve();
                OnConnectResolved(addr);
            }
            else
            {
                addr.Resolve(new AddressResolved(OnConnectResolved));
            }
        }

        /// <summary>
        /// Address resolution finished.  Try connecting.
        /// </summary>
        /// <param name="addr"></param>
        private void OnConnectResolved(Address addr)
        {
            // Debug.WriteLine("connectresolved: " + addr.ToString());
            lock (this)
            {
                if (State != SocketState.Resolving)
                {
                    // closed in the mean time.   Probably not an error.
                    return;
                }
                if ((addr == null) || (addr.IP == null) || (addr.Endpoint == null))
                {
                    FireError(new AsyncSocketConnectionException("Bad host: " + addr.Hostname));
                    return;
                }


                if (m_watcher != null)
                    m_watcher.RegisterSocket(this);

                m_addr = addr;
                State = SocketState.Connecting;

                if (Socket.OSSupportsIPv6 && (m_addr.Endpoint.AddressFamily == AddressFamily.InterNetworkV6))
                {
                    // Debug.WriteLine("ipv6");
                    m_sock = new Socket(AddressFamily.InterNetworkV6,
                        SocketType.Stream,
                        ProtocolType.Tcp);
                }
                else
                {
                    // Debug.WriteLine("ipv4");
                    m_sock = new Socket(AddressFamily.InterNetwork,
                        SocketType.Stream,
                        ProtocolType.Tcp);
                }

                // well, of course this isn't right.
                m_sock.SetSocketOption(SocketOptionLevel.Socket,
                    SocketOptionName.ReceiveBuffer,
                    4 * m_buf.Length);
            }

            if (m_synch)
            {
                try
                {
                    m_sock.Connect(m_addr.Endpoint);
                }
                catch (SocketException ex)
                {
                    FireError(ex);
                    return;
                }

                if (m_sock.Connected)
                {
                    // TODO: check to see if this Mono bug is still valid
#if __MonoCS__
                    m_sock.Blocking = true;
                    m_stream = new NetworkStream(m_sock);
                    m_sock.Blocking = false;
#else
                    m_stream = new NetworkStream(m_sock);
#endif
                    if (m_secureProtocol != SslProtocols.None)
                        StartTLS();

                    lock (this)
                    {
                        State = SocketState.Connected;
                    }
                    m_listener.OnConnect(this);
                }
                else
                {
                    AsyncClose();
                    FireError(new AsyncSocketConnectionException("could not connect"));
                }
            }
            else
            {
#if __MonoCS__
                m_sock.Blocking = false;
#endif
                m_sock.BeginConnect(m_addr.Endpoint, new AsyncCallback(ExecuteConnect), null);
            }
        }

        /// <summary>
        /// Validate the server cert.  SSLPolicyErrors will be
        /// pre-filled with the errors you got.
        ///
        /// If there is an error in the cert, OnIvalidCertificate will be called.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="certificate"></param>
        /// <param name="chain"></param>
        /// <param name="sslPolicyErrors"></param>
        /// <returns></returns>
        protected bool ValidateServerCertificate(object sender,
                                                 X509Certificate certificate,
                                                 X509Chain chain,
                                                 SslPolicyErrors sslPolicyErrors)
        {
            // Note: Don't write servers with Jabber-Net, please.  :)
            if (m_server)
            {
                return true;
            }

            if (sslPolicyErrors == SslPolicyErrors.None)
                return true;

            if ((sslPolicyErrors & (sslPolicyErrors ^ AllowedSSLErrors)) == SslPolicyErrors.None)
            {
                // Huh.  Maybe there should be a listener method for this.
                return true;
            }

            if (m_listener.OnInvalidCertificate(this, certificate, chain, sslPolicyErrors))
                return true;

            Debug.WriteLine("Certificate error: {0}", sslPolicyErrors.ToString());

            // Do not allow this client to communicate with unauthenticated servers.
            return false;
        }

        /// <summary>
        /// Start TLS processing on an open socket.
        /// </summary>
        public override void StartTLS()
        {
            // we're really doing start-tls.
            if (m_secureProtocol == SslProtocols.None)
                m_secureProtocol = SslProtocols.Tls;

            m_stream = m_sslStream = new SslStream(m_stream, false, ValidateServerCertificate, ChooseClientCertificate);

            if (m_server)
            {
                if (m_cert == null)
                {
                    FireError(new InvalidOperationException("Must set Certificate for server SSL"));
                    Close();
                    return;
                }
                // TODO: surface these as params
                m_sslStream.AuthenticateAsServer(m_cert, m_requireClientCert, m_secureProtocol, false);
            }
            else
            {
                if ((m_watcher != null) && (m_watcher.LocalCertificate != null))
                    m_cert = m_watcher.LocalCertificate;

                X509CertificateCollection certs = null;
                if (m_cert != null)
                {
                    certs = new X509Certificate2Collection();
                    certs.Add(m_cert);
                }
                try
                {
                    m_sslStream.AuthenticateAsClient(m_hostid, certs, m_secureProtocol, false);
                }
                catch (Exception ex)
                {
                    FireError(ex);
                    //Close();
                    //throw;
                }
            }
        }

        /// <summary>
        /// Is the connection mutually authenticated?  (was there a good client cert, etc.)
        /// </summary>
        public bool IsMutuallyAuthenticated
        {
            get
            {
                if (m_sslStream == null)
                    return false;
                return m_sslStream.IsMutuallyAuthenticated;
            }
        }

        /// <summary>
        /// Does the server require a client cert?  If not, the client cert won't be sent.
        /// </summary>
        public bool RequireClientCert
        {
            get { return m_requireClientCert; }
            set { m_requireClientCert = value; }
        }

        /// <summary>
        /// Start XEP-138 compression on this socket.
        /// </summary>
        public override void StartCompression()
        {
            m_stream = new bedrock.io.ZlibStream(m_stream, ComponentAce.Compression.Libs.zlib.zlibConst.Z_FULL_FLUSH);
        }

        /// <summary>
        /// Connection complete.
        /// </summary>
        /// <remarks>This is called solely by an async socket thread</remarks>
        /// <param name="ar"></param>
        private void ExecuteConnect(IAsyncResult ar)
        {
            Debug.WriteLine("ExecuteConnect");
            lock (this)
            {
                try
                {
                    m_sock.EndConnect(ar);
                }
                catch (SocketException e)
                {
                    if (State != SocketState.Connecting)
                    {
                        // closed in the mean time.   Probably not an error.
                        return;
                    }
                    FireError(e);
                    return;
                }
                if (m_sock.Connected)
                {
                    // TODO: Check to see if this Mono bug is still valid.
                    // TODO: check to see if blocking should be turned back on after StartTLS.
#if __MonoCS__
                    m_sock.Blocking = true;
                    m_stream = new NetworkStream(m_sock);
                    m_sock.Blocking = false;
#else
                    m_stream = new NetworkStream(m_sock);
#endif

                    if (m_secureProtocol != SslProtocols.None)
                    {
                        try
                        {
                            StartTLS();
                        }
                        catch (Exception e)
                        {
                            FireError(e);
                            AsyncClose();
                            return;
                        }
                    }

                    State = SocketState.Connected;
                    m_listener.OnConnect(this);
                }
                else
                {
                    FireError(new AsyncSocketConnectionException("could not connect"));
                    AsyncClose();
                }
            }
        }

        private bool SyncRead()
        {
            int count = m_stream.Read(m_buf, 0, m_buf.Length);

            if (count > 0)
            {
                return m_listener.OnRead(this, m_buf, 0, count);
            }
            Close();
            return false;
        }

        /// <summary>
        /// Start an async read from the socket.  Listener.OnRead() is
        /// eventually called when data arrives.
        /// </summary>
        public override void RequestRead()
        {
            try
            {
                if (m_synch)
                {
                    lock (this)
                    {
                        if (State != SocketState.Connected)
                        {
                            throw new InvalidOperationException("Socket not connected.");
                        }
                    }

                    while (SyncRead())
                    {
                        ;
                    }
                    return;
                }

                lock (this)
                {
                    if (m_reading)
                    {
                        throw new InvalidOperationException("Cannot call RequestRead while another read is pending.");
                    }
                    if (State != SocketState.Connected)
                    {
                        throw new InvalidOperationException("Socket not connected.");
                    }

                    m_reading = true;
                }
                m_stream.BeginRead(m_buf, 0, m_buf.Length, new AsyncCallback(GotData), null);
            }
            catch (AuthenticationException)
            {
                Close();
                // don't throw.  this gets caught elsewhere.
            }
            catch (SocketException e)
            {
                Close();

                // 10053 = An established connection was aborted by the
                //         software in your host machine.
                // 10054 = An existing connection was forcibly closed
                //         by the remote host.
                if ((e.ErrorCode != 10053) &&
                    (e.ErrorCode != 10054))
                {
                    throw;
                }
            }
            catch (IOException)
            {
                Close();
            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception in RequestRead: " + e.ToString());
                Close();
                throw e;
            }
        }

        /// <summary>
        /// Some data arrived.
        /// </summary>
        /// <param name="ar"></param>
        protected virtual void GotData(IAsyncResult ar)
        {
            lock (this)
            {
                m_reading = false;
            }

            int count;
            try
            {
                count = m_stream.EndRead(ar);
            }
            catch (SocketException e)
            {
                AsyncClose();

                // closed in middle of read
                if (e.ErrorCode != 64)
                {
                    FireError(e);
                }
                return;
            }
            catch(ObjectDisposedException)
            {
                //object already disposed, just exit
                return;
            }
            catch (Exception e)
            {
                AsyncClose();
                FireError(e);
                return;
            }
            if (count > 0)
            {
                //byte[] ret = new byte[count];
                //Buffer.BlockCopy(m_buf, 0, ret, 0, count);

                if (m_listener.OnRead(this, m_buf, 0, count) &&
                    (m_state == SocketState.Connected))
                {
                    RequestRead();
                }
            }
            else
            {
                AsyncClose();
            }
        }

        /// <summary>
        /// Async write to the socket.  Listener.OnWrite will be
        /// called eventually when the data has been written.  A
        /// trimmed copy is made of the data, internally.
        /// </summary>
        /// <param name="buf">Buffer to output</param>
        /// <param name="offset">Offset into buffer</param>
        /// <param name="len">Number of bytes to output</param>
        public override void Write(byte[] buf, int offset, int len)
        {
            lock (this)
            {
                if (State != SocketState.Connected)
                {
                    return;
                    //throw new InvalidOperationException("Socket must be connected before writing.  Current state: " + State.ToString());
                }

                try
                {
                    if (m_synch)
                    {
                        m_stream.Write(buf, offset, len);
                        m_listener.OnWrite(this, buf, offset, len);
                    }
                    else
                    {

                        if (m_writing)
                        {
                            // already writing.  save this for later.
                            m_pending.Write(buf, offset, len);
                        }
                        else
                        {
                            m_writing = true;
                            // make copy, since we might be a while in async-land
                            byte[] ret = new byte[len];
                            Buffer.BlockCopy(buf, offset, ret, 0, len);

                            m_stream.BeginWrite(ret, 0, ret.Length,
                                                new AsyncCallback(WroteData),
                                                ret);
                        }
                    }
                }
                catch (SocketException e)
                {
                    Close();

                    // closed in middle of write
                    if (e.ErrorCode != 10054)
                    {
                        FireError(e);
                    }
                    return;
                }
                catch (Exception e)
                {
                    Close();
                    FireError(e);
                    return;
                }
            }
        }

        /// <summary>
        /// Data was written.
        /// </summary>
        /// <param name="ar"></param>
        private void WroteData(IAsyncResult ar)
        {
            try
            {
                m_stream.EndWrite(ar);
            }
            catch (SocketException)
            {
                AsyncClose();
                return;
            }
            catch (ObjectDisposedException)
            {
                AsyncClose();
                return;
            }
            catch (Exception e)
            {
                AsyncClose();
                FireError(e);
                return;
            }

            lock (this)
            {
                m_writing = false;
            }
            byte[] buf = (byte[])ar.AsyncState;
            m_listener.OnWrite(this, buf, 0, buf.Length);

            if (m_pending.Length > 0)
            {
                buf = m_pending.ToArray();
                m_pending.SetLength(0L);
                Write(buf);
            }
        }

        /// <summary>
        /// Close the socket.  This is NOT async.  .Net doesn't have
        /// async closes.  But, it can be *called* async, particularly
        /// from GotData.  Attempts to do a shutdown() first.
        /// </summary>
        public override void Close()
        {
            Debug.WriteLine("Close");
            lock (this)
            {
                /*
                switch (State)
                {
                case State.Closed:
                    throw new InvalidOperationException("Socket already closed");
                case State.Closing:
                    throw new InvalidOperationException("Socket already closing");
                }
                */

                SocketState oldState = State;

                if (m_sock.Connected)
                {
                    State = SocketState.Closing;
                }

                if (m_stream != null)
                    m_stream.Close();
                else
                {
                    try
                    {
                        m_sock.Close();
                    }
                    catch { }
                }

                if (oldState <= SocketState.Connected)
                    m_listener.OnClose(this);

                if (m_watcher != null)
                    m_watcher.CleanupSocket(this);

                State = SocketState.Closed;
            }
        }


        /// <summary>
        /// Close, called from async places, so that Errors get fired,
        /// appropriately.
        /// </summary>
        protected void AsyncClose()
        {
            try
            {
                Close();
            }
            catch(Exception e)
            {
                FireError(e);
            }
        }

        /// <summary>
        /// Error occurred in the class.  Send to Listener.
        /// </summary>
        /// <param name="e"></param>
        protected void FireError(Exception e)
        {
            lock (this)
            {
                State = SocketState.Error;
            }
            if (e is SocketException)
            {
                Debug.WriteLine("Sock errno: " + ((SocketException) e).ErrorCode);
            }
            if (m_watcher != null)
                m_watcher.CleanupSocket(this);
            m_listener.OnError(this, e);
        }


        /// <summary>
        /// Return a string representation of this socket
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "AsyncSocket " + m_sock.LocalEndPoint + "->" +
                m_sock.RemoteEndPoint;
        }

        /// <summary>
        /// In case SocketWatcher wants to use a HashTable.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_id.GetHashCode();
        }

        #region IComparable
        int IComparable.CompareTo(object val)
        {
            if (val == null)
                return 1;

            AsyncSocket sock = val as AsyncSocket;
            if ((object)sock == null)
                throw new ArgumentException("value compared to is not an AsyncSocket", "val");

            return this.m_id.CompareTo(sock.m_id);
        }

        /// <summary>
        /// IComparable's need to implement Equals().  This checks the
        /// guid's for each socket to see if they are the same.
        /// </summary>
        /// <param name="val">The AsyncSocket to check against.</param>
        /// <returns></returns>
        public override bool Equals(object val)
        {
            AsyncSocket sock = val as AsyncSocket;
            if (sock == null)
                return false;
            return (this.m_id == sock.m_id);
        }

        /// <summary>
        /// IComparable's need to implement ==.  Checks for guid equality.
        /// </summary>
        /// <param name="one">First socket to compare</param>
        /// <param name="two">Second socket to compare</param>
        /// <returns></returns>
        public static bool operator==(AsyncSocket one, AsyncSocket two)
        {
            if ((object)one == null)
                return ((object)two == null);
            if ((object)two == null)
                return false;

            return (one.m_id == two.m_id);
        }

        /// <summary>
        /// IComparable's need to implement comparison operators.
        /// Checks compares guids.
        /// </summary>
        /// <param name="one">First socket to compare</param>
        /// <param name="two">Second socket to compare</param>
        /// <returns></returns>
        public static bool operator!=(AsyncSocket one, AsyncSocket two)
        {
            if ((object)one == null)
                return ((object)two != null);
            if ((object)two == null)
                return true;

            return (one.m_id != two.m_id);
        }

        /// <summary>
        /// IComparable's need to implement comparison operators.  Checks compares guids.
        /// </summary>
        /// <param name="one">First socket to compare</param>
        /// <param name="two">Second socket to compare</param>
        /// <returns></returns>
        public static bool operator<(AsyncSocket one, AsyncSocket two)
        {
            if ((object)one == null)
            {
                return ((object)two != null);
            }
            return (((IComparable)one).CompareTo(two) < 0);
        }
        /// <summary>
        /// IComparable's need to implement comparison operators.
        /// Checks compares guids.
        /// </summary>
        /// <param name="one">First socket to compare</param>
        /// <param name="two">Second socket to compare</param>
        /// <returns></returns>
        public static bool operator<=(AsyncSocket one, AsyncSocket two)
        {
            if ((object)one == null)
                return true;

            return (((IComparable)one).CompareTo(two) <= 0);
        }
        /// <summary>
        /// IComparable's need to implement comparison operators.
        /// Checks compares guids.
        /// </summary>
        /// <param name="one">First socket to compare</param>
        /// <param name="two">Second socket to compare</param>
        /// <returns></returns>
        public static bool operator>(AsyncSocket one, AsyncSocket two)
        {
            if ((object)one == null)
                return false;
            return (((IComparable)one).CompareTo(two) > 0);
        }
        /// <summary>
        /// IComparable's need to implement comparison operators.  Checks compares guids.
        /// </summary>
        /// <param name="one">First socket to compare</param>
        /// <param name="two">Second socket to compare</param>
        /// <returns></returns>
        public static bool operator>=(AsyncSocket one, AsyncSocket two)
        {
            if ((object)one == null)
            {
                return (two == null);
            }
            return (((IComparable)one).CompareTo(two) >= 0);
        }

        #endregion

        /// <summary>
        /// Retrieve the socketwatcher used by this instance of AsyncSocket
        /// </summary>
        public SocketWatcher SocketWatcher
        {
            get
            {
                return m_watcher;
            }
        }
    }
}
