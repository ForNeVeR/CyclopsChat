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

namespace jabber.protocol.stream
{
    /// <summary>
    /// Session start after binding
    /// </summary>
    [SVN(@"$Id: Session.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Session : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Session(XmlDocument doc) :
            base("", new XmlQualifiedName("session", jabber.protocol.URI.SESSION), doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Session(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }
    }
}
