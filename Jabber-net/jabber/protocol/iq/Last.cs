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
     *  <iq id='l4' type='result' from='user@host'>
     *    <query xmlns='jabber:iq:last' seconds='903'>
     *      Heading home
     *    </query>
     *  </iq>
     */
    /// <summary>
    /// IQ packet with an Last query element inside.
    /// </summary>
    [SVN(@"$Id: Last.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class LastIQ : jabber.protocol.client.TypedIQ<Last>
    {
        /// <summary>
        /// Create a Last IQ
        /// </summary>
        /// <param name="doc"></param>
        public LastIQ(XmlDocument doc) : base(doc)
        {
        }
    }

    /// <summary>
    /// A Last query element, which requests the last activity from an entity.
    /// </summary>
    [SVN(@"$Id: Last.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Last : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Last(XmlDocument doc) : base("query", URI.LAST, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Last(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        /// The message inside the Last element.
        /// </summary>
        public string Message
        {
            get { return this.InnerText; }
            set { this.InnerText = value; }
        }

        /// <summary>
        /// How many seconds since the last activity.
        /// </summary>
        public int Seconds
        {
            get { return GetIntAttr("seconds");}
            set { SetAttribute("seconds", value.ToString()); }
        }
    }
}
