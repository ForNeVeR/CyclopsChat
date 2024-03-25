using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Cyclops.Core;

namespace Cyclops
{
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Check sequence for null or empty
        /// </summary>
        public static bool IsNullOrEmpty(this IEnumerable enumerable)
        {
            if (enumerable == null || !enumerable.Any())
                return true;
            return false;
        }

        /// <summary>
        /// Does collection contain any elements
        /// </summary>
        public static bool Any(this IEnumerable enumerable)
        {
            foreach (object item in enumerable)
                return true; //just one call of MoveNext is required
            return false;
        }

        public static IEnumerable<T> Clone<T>(this IEnumerable<T> sequence)
        {
            foreach (T item in sequence)
                yield return item;
        }

        /// <summary>
        /// Quick foreach 
        /// </summary>
        public static IEnumerable<T> ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (T item in source)
                action(item);
            return source;
        }

        /// <summary>
        /// Convert a collection into ObservableCollection
        /// </summary>
        public static ObservableCollection<T> ToObservables<T>(this IEnumerable<T> source)
        {
            return new ObservableCollection<T>(source);
        }

        /// <summary>
        /// Convert a collection into string 
        /// </summary>
        public static string CollectionToString<T, TR>(this IEnumerable<T> source, Func<T, TR> selector,
                                                       string separator = "; ")
        {
            bool isRefType = !typeof (TR).IsValueType;
            string result = string.Empty;

            foreach (T item in source)
            {
                TR value = selector(item);
                if (isRefType && value == null) //ignore null items if reftype
                    continue;

                result += value + separator;
            }

            //remove last separator
            if (!string.IsNullOrEmpty(result))
                result = result.Remove(result.Length - separator.Length);
            return result;
        }

        /// <summary>
        /// Set one-way synchronization between two collections
        /// </summary>
        public static void SynchronizeWith<T, TR, TKey>(this IObservableCollection<T> source,
                                                  ObservableCollection<TR> destination, 
                                                  Func<T, TR> creator,
                                                  Func<T, TKey> sourceKey,
                                                  Func<TR, TKey> destKey)
        {
            source.ForEach(item => destination.Add(creator(item)));
            source.CollectionChanged += (s, e) =>
                                            {
                                                if (e.Action == NotifyCollectionChangedAction.Add)
                                                    e.NewItems.OfType<T>().ForEach(
                                                        item => destination.Add(creator(item)));
                                                if (e.Action == NotifyCollectionChangedAction.Remove && sourceKey != null && destKey != null)
                                                {
                                                    foreach (var item in e.OldItems.OfType<T>().Join(destination, sourceKey, destKey, (src, dst) => dst))
                                                    {
                                                        destination.Remove(item);
                                                        //remover(item);
                                                    }
                                                }
                                            };
        }

        /// <summary>
        /// Set one-way synchronization between two collections
        /// </summary>
        public static void SynchronizeWith<T, TR>(this IObservableCollection<T> source,
                                                  ObservableCollection<TR> destination,
                                                  Func<T, TR> creator)
        {
            SynchronizeWith<T, TR, T>(source, destination, creator, null, null);
        }
    }
}