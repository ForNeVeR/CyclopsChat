using System;
using Cyclops.Core.Configuration;
using Cyclops.Core.CustomEventArgs;

namespace Cyclops.Core.Registration
{
    public interface IRegistrationManager : ISessionHolder
    {
        void RegisterNewUserAsync(ConnectionConfig connectionConfig, Action<RegistrationEventArgs> callback);
    }
}
