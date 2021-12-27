using Cyclops.Core.Modularity;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.JabberNet.Data;
using Microsoft.Practices.Unity;

namespace Cyclops.Xmpp.JabberNet.Composition
{
    public sealed class Module : ModuleBase
    {
        public override void Initialize(IUnityContainer container)
        {
            container.RegisterType<IXmppDataExtractor, JabberNetDataExtractor>();
        }
    }
}
