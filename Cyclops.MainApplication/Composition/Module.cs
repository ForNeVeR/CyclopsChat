using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Threading;
using Cyclops.Core.Modularity;
using Cyclops.Core.Resource;
using Unity;

namespace Cyclops.MainApplication.Composition
{
    public sealed class Module : ModuleBase
    {
        public override void Initialize(IUnityContainer container)
        {
            container
                .RegisterInstance<Action<Action>>((action) => InvokeAsyncIfRequired(Application.Current.Dispatcher, action))
                .RegisterType<ISynchronizeInvoke, SynchronizeInvokeImpl>();
        }

        public static void InvokeAsyncIfRequired(Dispatcher dispatcher, Action action)
        {
            if (dispatcher.CheckAccess())
                action();
            else
                dispatcher.InvokeAsync(action);
        }
    }
}
