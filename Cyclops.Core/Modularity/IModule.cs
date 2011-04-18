using System.Collections.Generic;
using Microsoft.Practices.Unity;

namespace Cyclops.Core.Modularity
{
    public interface IModule
    {
        IEnumerable<IModule> Children { get; }

        void Initialize(IUnityContainer container);
    }
}