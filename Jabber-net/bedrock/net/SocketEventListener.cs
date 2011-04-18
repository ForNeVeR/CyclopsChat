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

using bedrock.util;
namespace bedrock.net
{
    /// <summary>
    /// Interface class for Socket events. Any object which
    /// implements these interfaces is eligible to recieve Socket
    /// events.  This is an interface instead of events in order
    /// to preserve symmetry with libbedrock.
    /// </summary>
    [SVN(@"$Id: SocketEventListener.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public interface ISocketEventListener
    {
        /// <summary>
        /// An accept socket is about to be bound, or a connect socket is about to connect,
        /// or an incoming socket just came in.  Use this as an opportunity to
        /// </summary>
        /// <param name="newSock">The new socket that is about to be connected.</param>
        void OnInit(BaseSocket newSock);

        /// <summary>
        /// We accepted a socket, and need to get a listener.
        /// If the return value is null, then the socket will be closed,
        /// and RequestAccept will ALWAYS be called.
        /// </summary>
        /// <param name="newSock">The new socket.</param>
        /// <returns>The listener for the *new* socket, as compared to
        /// the listener for the *listen* socket</returns>
        ISocketEventListener GetListener(BaseSocket newSock);

        /// <summary>
        /// A new incoming connection was accepted.
        /// </summary>
        /// <param name="newsocket">Socket for new connection.</param>
        /// <returns>true if RequestAccept() should be called automatically again</returns>
        bool OnAccept(BaseSocket newsocket);
        /// <summary>
        /// Outbound connection was connected.
        /// </summary>
        /// <param name="sock">Connected socket.</param>
        void OnConnect(BaseSocket sock);
        /// <summary>
        /// Connection was closed.
        /// </summary>
        /// <param name="sock">Closed socket.  Already closed!</param>
        void OnClose(BaseSocket sock);
        /// <summary>
        /// An error happened in processing.  The socket is no longer open.
        /// </summary>
        /// <param name="sock">Socket in error</param>
        /// <param name="ex">Exception that caused the error</param>
        void OnError(BaseSocket sock, Exception ex);
        /// <summary>
        /// Bytes were read from the socket.
        /// </summary>
        /// <param name="sock">The socket that was read from.</param>
        /// <param name="buf">The bytes that were read.</param>
        /// <param name="offset">Offset into the buffer to start at</param>
        /// <param name="length">Number of bytes to use out of the buffer</param>
        /// <returns>true if RequestRead() should be called automatically again</returns>
        bool OnRead (BaseSocket sock, byte[] buf, int offset, int length);
        /// <summary>
        /// Bytes were written to the socket.
        /// </summary>
        /// <param name="sock">The socket that was written to.</param>
        /// <param name="buf">The bytes that were written.</param>
        /// <param name="offset">Offset into the buffer to start at</param>
        /// <param name="length">Number of bytes to use out of the buffer</param>
        void OnWrite(BaseSocket sock, byte[] buf, int offset, int length);

        /// <summary>
        /// An invalid peer certificate was sent during SSL/TLS neogtiation.
        /// </summary>
        /// <param name="sock">The socket that experienced the error</param>
        /// <param name="certificate">The bad certificate</param>
        /// <param name="chain">The chain of CAs for the cert</param>
        /// <param name="sslPolicyErrors">A bitfield for the erorrs in the certificate.</param>
        /// <returns>True if the cert should be accepted anyway.</returns>
        bool OnInvalidCertificate(BaseSocket sock,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors);
    }
    /// <summary>
    /// Default, empty implementation of ISocketEventListener
    /// </summary>
    [SVN(@"$Id: SocketEventListener.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class SocketEventListener : ISocketEventListener
    {
        #region Implementation of ISocketEventListener
        /// <summary>
        /// An accept socket is about to be bound, or a connect socket is about to connect,
        /// or an incoming socket just came in.  Use this as an opportunity to
        /// </summary>
        /// <param name="newSock">The new socket that is about to be connected.</param>
        public virtual void OnInit(BaseSocket newSock)
        {
        }

        /// <summary>
        /// We accepted a socket, and need to get a listener.
        /// If the return value is null, then the socket will be closed,
        /// and RequestAccept will ALWAYS be called.
        /// </summary>
        /// <param name="newSock">The new socket.</param>
        /// <returns>The listener for the *new* socket, as compared to
        /// the listener for the *listen* socket</returns>
        public virtual ISocketEventListener GetListener(BaseSocket newSock)
        {
            return this;
        }

        /// <summary>
        /// A new incoming connection was accepted.
        /// </summary>
        /// <param name="newsocket">Socket for new connection.</param>
        /// <returns>true if RequestAccept() should be called automatically again</returns>
        public virtual bool OnAccept(BaseSocket newsocket)
        {
            return true;
        }

        /// <summary>
        /// Outbound connection was connected.
        /// </summary>
        /// <param name="sock">Connected socket.</param>
        public virtual void OnConnect(BaseSocket sock)
        {
        }

        /// <summary>
        /// Connection was closed.
        /// </summary>
        /// <param name="sock">Closed socket.  Already closed!</param>
        public virtual void OnClose(BaseSocket sock)
        {
        }

        /// <summary>
        /// An error happened in processing.  The socket is no longer open.
        /// </summary>
        /// <param name="sock">Socket in error</param>
        /// <param name="ec">Exception that caused the error</param>
        public virtual void OnError(BaseSocket sock, System.Exception ec)
        {
        }

        /// <summary>
        /// Bytes were read from the socket.
        /// </summary>
        /// <param name="sock">The socket that was read from.</param>
        /// <param name="buf">The bytes that were read.</param>
        /// <returns>true if RequestRead() should be called automatically again</returns>
        /// <param name="offset">Offset into the buffer to start at</param>
        /// <param name="length">Number of bytes to use out of the buffer</param>
        public virtual bool OnRead(BaseSocket sock, byte[] buf, int offset, int length)
        {
            return true;
        }

        /// <summary>
        /// Bytes were written to the socket.
        /// </summary>
        /// <param name="sock">The socket that was written to.</param>
        /// <param name="buf">The bytes that were written.</param>
        /// <param name="offset">Offset into the buffer to start at</param>
        /// <param name="length">Number of bytes to use out of the buffer</param>
        public virtual void OnWrite(BaseSocket sock, byte[] buf, int offset, int length)
        {
        }

        /// <summary>
        /// An invalid peer certificate was sent during SSL/TLS neogtiation.
        /// </summary>
        /// <param name="sock">The socket that experienced the error</param>
        /// <param name="certificate">The bad certificate</param>
        /// <param name="chain">The chain of CAs for the cert</param>
        /// <param name="sslPolicyErrors">A bitfield for the erorrs in the certificate.</param>
        /// <returns>True if the cert should be accepted anyway.</returns>
        public virtual bool OnInvalidCertificate(BaseSocket sock,
            System.Security.Cryptography.X509Certificates.X509Certificate certificate,
            System.Security.Cryptography.X509Certificates.X509Chain chain,
            System.Net.Security.SslPolicyErrors sslPolicyErrors)
        {
            return false;
        }
        #endregion
    }
}
