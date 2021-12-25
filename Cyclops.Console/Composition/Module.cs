using Cyclops.Core;
using Cyclops.Core.Modularity;
using Microsoft.Practices.Unity;

namespace Cyclops.Console.Composition
{
    public sealed class Module : ModuleBase
    {
        public override void Initialize(IUnityContainer container)
        {
            container
                .RegisterType<IDebugWindow, ConsoleWindow>();
        }
    }
}
