/* --------------------------------------------------------------------------
 * Copyrights
 *
 * Portions created by or assigned to Cursive Systems, Inc. are
 * Copyright (c) 2002-2008 Cursive Systems, Inc.  All Rights Reserved.  Contact
 * information for Cursive Systems, Inc. is available at
 * http://www.cursive.net/.
 *
 * License
 *
 * Jabber-Net is licensed under the LGPL.
 * See LICENSE.txt for details.
 * --------------------------------------------------------------------------*/
using System;

using System.Collections;
using bedrock.util;

namespace bedrock.collections
{
    /// <summary>
    /// Set operations.
    /// </summary>
    [SVN(@"$Id: ISet.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public interface ISet : ICollection
    {
        /// <summary>
        /// Add an object to the set
        /// </summary>
        /// <param name="o">The object to add</param>
        /// <exception cref="ArgumentException">object was already in the set.</exception>
        void Add(object o);

        /// <summary>
        /// Remove the given object from the set.  If the object is not in the set, this is a no-op.
        /// </summary>
        /// <param name="o">The object to remove.</param>
        void Remove(object o);

        /// <summary>
        /// Removes all items from the set.
        /// </summary>
        void Clear();

        /// <summary>
        /// Is the given object in the set?
        /// </summary>
        /// <param name="o">The object to search for.</param>
        /// <returns>True if the object is in the set.</returns>
        bool Contains(object o);

        /// <summary>
        /// Returns a new collection that contains all of the items that
        /// are in this set or the other set.
        /// </summary>
        /// <param name="other">Second set to combine with this one.</param>
        /// <returns>Combined set.</returns>
        ISet Union(ISet other);

        /// <summary>
        /// Return a new collection that contains all of the items that
        /// are in this list *and* the other set.
        /// </summary>
        ISet Intersection(ISet other);
    }
}
