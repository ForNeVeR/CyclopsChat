using System.Windows;
using System.Windows.Threading;
using Cyclops.Core.Modularity;
using Microsoft.Practices.Unity;

namespace Cyclops.MainApplication.Composition
{
    public sealed class Module : ModuleBase
    {
        public override void Initialize(IUnityContainer container)
        {
            container.RegisterInstance<Dispatcher>(Application.Current.Dispatcher);
        }
    }
}