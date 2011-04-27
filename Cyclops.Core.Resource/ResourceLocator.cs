using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops.Core.Composition;
using Cyclops.Core.Modularity;
using Microsoft.Practices.ServiceLocation;

namespace Cyclops.Core.Resource
{
    internal class ResourceLocator
    {
        private static readonly IServiceLocator serviceLocator = Bootstrapper.Run(
            new IModule[]
                {
                    new Module(),
                    new Core.Resource.Composition.Module(),
                });

        internal static IServiceLocator ServiceLocator
        {
            get { return serviceLocator; }
        }
    }
}
