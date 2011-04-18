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
    /// Stream error packet.
    /// </summary>
    [SVN(@"$Id: Error.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Error : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Error(XmlDocument doc) : base("stream", new XmlQualifiedName("error", URI.STREAM), doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Error(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The error message
        /// </summary>
        public string Message
        {
            get { return this.InnerText; }
            set { this.InnerXml = value; }
        }
    }
}
