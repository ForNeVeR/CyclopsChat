using Cyclops.Configuration;
using Cyclops.Core;
using Cyclops.Core.Security;
using Cyclops.Xmpp.Registration;
using jabber;
using jabber.client;
using jabber.connection;
using jabber.protocol.client;
using jabber.protocol.stream;

namespace Cyclops.Xmpp.JabberNet.Registration;

public class JabberNetRegistrationManager : IRegistrationManager
{
    private readonly ILogger logger;
    private readonly IStringEncryptor encryptor;

    public JabberNetRegistrationManager(ILogger logger, IStringEncryptor encryptor)
    {
        this.logger = logger;
        this.encryptor = encryptor;
    }

    public async Task<RegistrationResult> RegisterNewUser(ConnectionConfig connectionConfig)
    {
        using var client = new JabberClient();
        try
        {
            client.AutoLogin = false;
            client[Options.SASL_MECHANISMS] = MechanismType.DIGEST_MD5;
            client.AutoLogin = false;
            client.Server = connectionConfig.Server;
            client.NetworkHost = connectionConfig.NetworkHost;
            client.Port = connectionConfig.Port;
            client.Password = encryptor.DecryptString(connectionConfig.EncodedPassword ?? "");
            client.User = connectionConfig.User;
            client.AutoRoster = false;
            client.Priority = -1;
            client.AutoPresence = false;

            var result = new TaskCompletionSource<RegistrationResult>();

            client.OnAuthError += (_, _) => OnAuthError(result);
            client.OnLoginRequired += _ => client.Register(new JID(client.User, client.Server, null));
            client.OnRegistered += (_, iq) => OnRegistered(client, iq, result);
            client.OnRegisterInfo += (_, _) => true;
            client.OnAuthenticate += (_) => OnAuthenticate(result);
            client.OnError += (_, error) => OnError(error, result);

            client.Connect();

            return await result.Task;
        }
        finally
        {
            client.Close();
        }
    }

    private void OnError(Exception ex, TaskCompletionSource<RegistrationResult> result)
    {
        result.SetResult(new RegistrationResult(ex.Message));
    }

    private void OnAuthenticate(TaskCompletionSource<RegistrationResult> result)
    {
        result.SetResult(new RegistrationResult());
    }

    private void OnAuthError(TaskCompletionSource<RegistrationResult> result)
    {
        result.SetResult(new RegistrationResult("Shit-happens-error: can't authenticate with created account."));
    }

    private void OnRegistered(JabberClient client, IQ iq, TaskCompletionSource<RegistrationResult> result)
    {
        if (iq.Error != null)
        {
            var message = iq.Error.Code switch
            {
                //TODO: replace with events (move to presentation layer)
                409 => "This name is already registered.",
                500 => "You can't register accounts within small time interval",
                _ => string.IsNullOrEmpty(iq.Error.Message) ? iq.Error.Condition : iq.Error.Message
            };

            result.SetResult(new RegistrationResult(message));
            return;
        }

        if (iq.Type == IQType.result)
            client.Login();
    }
}
