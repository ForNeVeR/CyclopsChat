using System.Windows.Threading;
using Cyclops.Core;
using Cyclops.Core.Composition;
using Cyclops.Core.Modularity;
using Cyclops.Core.Security;
using Cyclops.Core.Smiles;
using Microsoft.Practices.ServiceLocation;

namespace Cyclops.MainApplication
{
    public static class ChatObjectFactory
    {
        private static readonly IServiceLocator serviceLocator = Bootstrapper.Run(
            new IModule[]
                {
                    new Module(),
                    new Core.Resource.Composition.Module()
                });

        private static IUserSession session;

        public static IServiceLocator ServiceLocator
        {
            get { return serviceLocator; }
        }

        public static IUserSession GetSession()
        {
            if (session == null)
            {
                session = ServiceLocator.GetInstance<IUserSession>();
                session.Dispatcher = Dispatcher.CurrentDispatcher;
            }

            return session;
        }

        public static ILogger GetLogger()
        {
            return serviceLocator.GetInstance<ILogger>();
        }

        public static ISmilesManager GetSmilesManager()
        {
            return serviceLocator.GetInstance<ISmilesManager>();
        }
        
        public static IStringEncryptor GetStringEncryptor()
        {
            return serviceLocator.GetInstance<IStringEncryptor>();
        }
    }
}