using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Cyclops.Core
{
    public interface IObservableCollection<out T> : IEnumerable<T>, INotifyCollectionChanged
    {
    }
}