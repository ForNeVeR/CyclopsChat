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

namespace jabber.protocol.accept
{
    /// <summary>
    /// The handshake tag, including digest calculation.  Call SetAuth() to calculate
    /// the SHA1 hash.
    /// </summary>
    [SVN(@"$Id: Handshake.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Handshake : jabber.protocol.Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Handshake(XmlDocument doc) : base("handshake", doc)
        {
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Handshake(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// Set the auth information for this handshake tag,
        /// performing the digest operation.
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="streamID"></param>
        public void SetAuth(string secret, string streamID)
        {
            this.Digest = ShaHash(streamID, secret);
        }

        /// <summary>
        /// The digest.
        /// </summary>
        public string Digest
        {
            get { return this.InnerText; }
            set { this.InnerText = value; }
        }
    }
}
