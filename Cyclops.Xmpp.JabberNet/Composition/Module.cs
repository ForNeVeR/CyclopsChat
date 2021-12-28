using Cyclops.Core.Modularity;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.JabberNet.Data;
using Cyclops.Xmpp.JabberNet.Registration;
using Cyclops.Xmpp.Registration;
using Microsoft.Practices.Unity;

namespace Cyclops.Xmpp.JabberNet.Composition
{
    public sealed class Module : ModuleBase
    {
        public override void Initialize(IUnityContainer container)
        {
            container
                .RegisterType<IRegistrationManager, JabberNetRegistrationManager>()
                .RegisterType<IXmppDataExtractor, JabberNetDataExtractor>();
        }
    }
}
