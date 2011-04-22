using Cyclops.Core.Modularity;
using Cyclops.Core.Resource.Debug;
using Cyclops.Core.Resource.Security;
using Cyclops.Core.Resource.Smiles;
using Cyclops.Core.Security;
using Cyclops.Core.Smiles;
using jabber;
using Microsoft.Practices.Unity;

namespace Cyclops.Core.Resource.Composition
{
    public sealed class Module : ModuleBase
    {
        public override void Initialize(IUnityContainer container)
        {
            container
                .RegisterType<ILogger, FileLogger>()
                .RegisterType<ISmilesManager, SmilesManager>()
                .RegisterType<IStringEncryptor, TripleDesStringEncryptor>()
                .RegisterType<IEntityIdentifier, JID>()
                .RegisterType<IDebugWindow, RawXmppViewer>()
                .RegisterType<IUserSession, UserSession>();
        }
    }
}