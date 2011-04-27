using System.ComponentModel;

namespace Cyclops.Core
{
    public interface ISynchronizeInvokeHolder
    {
        ISynchronizeInvoke Invoker { get; set; }
    }
}