using Cyclops.Configuration;

namespace Cyclops.Xmpp.Registration;

public interface IRegistrationManager
{
    Task<RegistrationResult> RegisterNewUser(ConnectionConfig connectionConfig);
}
