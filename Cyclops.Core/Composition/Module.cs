using Cyclops.Core.Configuration;
using Cyclops.Core.Modularity;
using Microsoft.Practices.Unity;

namespace Cyclops.Core.Composition
{
    public sealed class Module : ModuleBase
    {
        public override void Initialize(IUnityContainer container)
        {
            container.RegisterType<ConnectionConfig>();
        }
    }
}