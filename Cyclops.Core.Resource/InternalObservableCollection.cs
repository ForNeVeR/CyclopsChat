using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace Cyclops.Core.Resource
{
    internal class InternalObservableCollection<T> : IObservableCollection<T>
    {
        private readonly List<T> innerList = new List<T>();

        internal void Add(T item)
        {
            innerList.Add(item);
            CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
        }

        internal void AddRange(IEnumerable<T> items)
        {
            innerList.AddRange(items);
            CollectionChanged(this,
                              new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, new List<T>(items)));
        }

        internal void Remove(T item)
        {
            int index = innerList.IndexOf(item);
            CollectionChanged(this,
                              new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item, index));
            innerList.Remove(item);
        }

        internal void Remove(Predicate<T> condition)
        {
            List<T> cloneList = innerList.Where(i => condition(i)).ToList();
            if (cloneList.Count == 0)
                return;

            cloneList.ForEach(Remove);
        }

        internal void Clear()
        {
            innerList.ToList().ForEach(Remove);
        }

        #region Implementation of IEnumerable

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>1</filterpriority>
        public IEnumerator<T> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        /// <filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        #endregion

        #region Implementation of INotifyCollectionChanged

        public event NotifyCollectionChangedEventHandler CollectionChanged = delegate { };

        #endregion
    }
}