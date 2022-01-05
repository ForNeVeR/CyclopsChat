using System.Windows;
using Cyclops.Core.Modularity;
using Unity;

namespace Cyclops.MainApplication.Composition
{
    public sealed class Module : ModuleBase
    {
        public override void Initialize(IUnityContainer container)
        {
            container.RegisterInstance(Application.Current.Dispatcher);
        }
    }
}
