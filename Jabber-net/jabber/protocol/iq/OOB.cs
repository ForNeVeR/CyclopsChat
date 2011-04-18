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
    /*
     * <iq type="set" to="horatio@denmark" from="sailor@sea" id="i_oob_001">
     *   <query xmlns="jabber:iq:oob">
     *     <url>http://denmark/act4/letter-1.html</url>
     *     <desc>There's a letter for you sir.</desc>
     *   </query>
     * </iq>
     */
    /// <summary>
    /// IQ packet with an oob query element inside.
    /// </summary>
    [SVN(@"$Id: OOB.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class OobIQ : jabber.protocol.client.TypedIQ<OOB>
    {
        /// <summary>
        /// Create an OOB IQ.
        /// </summary>
        /// <param name="doc"></param>
        public OobIQ(XmlDocument doc) : base(doc)
        {
        }
    }

    /// <summary>
    /// An oob query element for file transfer.
    /// </summary>
    [SVN(@"$Id: OOB.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class OOB : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public OOB(XmlDocument doc) : base("query", URI.OOB, doc)
        {
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public OOB(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// URL to send/receive from
        /// </summary>
        public string Url
        {
            get { return GetElem("url"); }
            set { SetElem("url", value); }
        }

        /// <summary>
        /// File description
        /// </summary>
        public string Desc
        {
            get { return GetElem("desc"); }
            set { SetElem("desc", value); }
        }
    }
}
