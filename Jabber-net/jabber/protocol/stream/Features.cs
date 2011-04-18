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
    /// Stream Features handler
    /// </summary>
    public delegate void FeaturesHandler(Object sender, Features feat);

    /// <summary>
    /// Stream features.  Will only be set by a version="1.0" or higher XMPP server.
    /// </summary>
    [SVN(@"$Id: Features.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Features : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Features(XmlDocument doc) :
            base("stream", new XmlQualifiedName("features", jabber.protocol.URI.STREAM), doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Features(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The starttls element, or null if none found.
        /// </summary>
        public StartTLS StartTLS
        {
            get { return this["starttls", jabber.protocol.URI.START_TLS] as StartTLS; }
            set { ReplaceChild(value); }
        }

        /// <summary>
        /// The SASL mechanisms, or null if none found.
        /// </summary>
        public Mechanisms Mechanisms
        {
            get { return this["mechanisms", jabber.protocol.URI.SASL] as Mechanisms; }
            set { ReplaceChild(value); }
        }

        /// <summary>
        /// The compression element, or null if none found.
        /// </summary>
        public Compression Compression
        {
            get { return this["compression", jabber.protocol.URI.COMPRESS_FEATURE] as Compression; }
        }
    }
}
