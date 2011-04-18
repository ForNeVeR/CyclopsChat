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

using System.Xml;

using bedrock.util;

namespace jabber.protocol.iq
{
    /// <summary>
    /// IQ packet with a version query element inside.
    /// </summary>
    [SVN(@"$Id: Version.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class VersionIQ : jabber.protocol.client.TypedIQ<Version>
    {
        /// <summary>
        /// Create a version IQ
        /// </summary>
        /// <param name="doc"></param>
        public VersionIQ(XmlDocument doc) : base(doc)
        {
        }
    }

    /// <summary>
    /// A time query element.
    /// </summary>
    [SVN(@"$Id: Version.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Version : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Version(XmlDocument doc) : base("query", URI.VERSION, doc)
        {
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Version(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Name of the entity.
        /// </summary>
        public string EntityName
        {
            get { return GetElem("name"); }
            set { SetElem("name", value); }
        }

        /// <summary>
        /// Enitity version.  (Version was a keyword, or something)
        /// </summary>
        public string Ver
        {
            get { return GetElem("version"); }
            set { SetElem("version", value); }
        }

        /// <summary>
        /// Operating system of the entity.
        /// </summary>
        public string OS
        {
            get { return GetElem("os"); }
            set { SetElem("os", value); }
        }
    }
}
