using System.Windows.Controls;
using System.Windows.Threading;
using Cyclops.Core;
using Cyclops.Core.Composition;
using Cyclops.Core.Modularity;
using Cyclops.Core.Registration;
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
                    new Core.Resource.Composition.Module(),
                    new Composition.Module()
                });

        private static IUserSession session;

        public static IServiceLocator ServiceLocator
        {
            get { return serviceLocator; }
        }

        public static IUserSession GetSession()
        {
            return session ?? (session = ServiceLocator.GetInstance<IUserSession>());
        }

        public static ILogger GetDebugLogger()
        {
            return serviceLocator.GetInstance<ILogger>();
        }

        public static IChatLogger GetChatLogger()
        {
            return serviceLocator.GetInstance<IChatLogger>();
        }

        public static ISmilesManager GetSmilesManager()
        {
            return serviceLocator.GetInstance<ISmilesManager>();
        }

        public static IRegistrationManager GetRegistrationManager()
        {
            return serviceLocator.GetInstance<IRegistrationManager>();
        }
        
        public static IStringEncryptor GetStringEncryptor()
        {
            return serviceLocator.GetInstance<IStringEncryptor>();
        }

        public static void ShowDebugWindow()
        {
            //serviceLocator.GetInstance<IDebugWindow>().ShowConsole(GetSession());
        }

        public static IChatObjectsValidator GetValidator()
        {
            return serviceLocator.GetInstance<IChatObjectsValidator>();
        }
    }
}