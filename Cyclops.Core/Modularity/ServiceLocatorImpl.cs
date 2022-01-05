using System;
using System.Collections.Generic;
using CommonServiceLocator;
using Unity;

namespace Cyclops.Core.Modularity
{
    internal sealed class ServiceLocatorImpl : ServiceLocatorImplBase
    {
        private readonly IUnityContainer unity;

        public ServiceLocatorImpl(IUnityContainer unity)
        {
            this.unity = unity;
        }

        protected override object DoGetInstance(Type serviceType, string key)
        {
            return unity.Resolve(serviceType, key);
        }

        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            return unity.ResolveAll(serviceType);
        }
    }
}
