using System;
using System.Windows.Threading;
using System.Xml;
using Cyclops.Core.Configuration;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Registration;
using Cyclops.Core.Security;
using jabber;
using jabber.client;
using jabber.connection;
using jabber.protocol.client;
using jabber.protocol.iq;
using jabber.protocol.stream;

namespace Cyclops.Core.Resource.Registration
{
    public class RegistrationManager : IRegistrationManager
    {
        private readonly IStringEncryptor encryptor;
        private readonly Dispatcher dispatcher;

        public RegistrationManager(IStringEncryptor encryptor, Dispatcher dispatcher)
        {
            this.encryptor = encryptor;
            this.dispatcher = dispatcher;
        }

        private JabberClient client = null;
        private Action<RegistrationEventArgs> callback = null;

        #region Implementation of ISessionHolder
        
        public void RegisterNewUserAsync(ConnectionConfig connectionConfig, Action<RegistrationEventArgs> callback)
        {
            this.callback = callback;

            client = new JabberClient();

            client.AutoLogin = false;
            if (dispatcher != null)
                client.InvokeControl = new SynchronizeInvokeImpl(dispatcher);
            client[Options.SASL_MECHANISMS] = MechanismType.DIGEST_MD5;
            client.AutoLogin = false;
            client.Server = connectionConfig.Server;
            client.NetworkHost = connectionConfig.NetworkHost;
            client.Port = connectionConfig.Port;
            client.Password = encryptor.DecryptString(connectionConfig.EncodedPassword);
            client.User = connectionConfig.User;
            client.AutoRoster = false;
            client.Priority = -1;
            client.AutoPresence = false;

            client.OnInvalidCertificate += (s, cert, c, p) => true;
            client.OnAuthError += ClientOnAuthError;
            client.OnLoginRequired += ClientOnLoginRequired;
            client.OnRegistered += ClientOnRegistered;
            client.OnRegisterInfo += ClientOnRegisterInfo;
            client.OnAuthenticate += ClientOnAuthenticate;
            client.OnError += ClientOnError;

            client.Connect();
        }
        
        private void ClientOnError(object sender, Exception ex)
        {
            callback(new RegistrationEventArgs(ex.Message));
        }

        private void ClientOnAuthenticate(object sender)
        {
            client.Close();
            callback(new RegistrationEventArgs());
        }

        private void ClientOnAuthError(object sender, XmlElement rp)
        {
            callback(new RegistrationEventArgs("Shit-happens-error: can't authenticate with created account."));
        }

        private bool ClientOnRegisterInfo(object sender, Register register)
        {    
            return true;
        }

        private void ClientOnRegistered(object sender, IQ iq)
        {    
            if (iq.Error != null)
            {
                string message = "";
                switch (iq.Error.Code)
                {
                    case 500:
                        message = "You can't register accounts within small time interval";
                        break;
                    default:
                        message = string.IsNullOrEmpty(iq.Error.Message) ? iq.Error.Condition : iq.Error.Message;
                        break;
                }

                callback(new RegistrationEventArgs(message));
                return;
            }


            if (iq.Type == IQType.result)
                client.Login();
        }

        private void ClientOnLoginRequired(object sender)
        {
            client.Register(new JID(client.User, client.Server, null));
        }

        #endregion
    }
}
