using Cyclops.Core.Modularity;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Registration;
using Cyclops.Xmpp.SharpXmpp.Client;
using Cyclops.Xmpp.SharpXmpp.Data;
using Cyclops.Xmpp.SharpXmpp.Registration;
using Microsoft.Practices.Unity;

namespace Cyclops.MainApplication.Composition;

public class SharpXmppClientModule : ModuleBase
{
    public override void Initialize(IUnityContainer container)
    {
        container
            .RegisterType<IXmppClient, SharpXmppClient>()
            .RegisterType<IRegistrationManager, SharpXmppRegistrationManager>()
            .RegisterType<IXmppDataExtractor, SharpXmppDataExtractor>();
    }
}
