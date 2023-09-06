using System.Collections.Generic;
using System.Linq;
using Unity;

namespace Cyclops.Core.Modularity
{
    public abstract class ModuleBase : IModule
    {
        #region IModule Members

        public virtual IEnumerable<IModule> Children => Enumerable.Empty<IModule>();

        public virtual void Initialize(IUnityContainer container)
        {
        }

        #endregion
    }
}
