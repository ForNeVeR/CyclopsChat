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
using System.Diagnostics;
using System.Text;
using System.Xml;

using bedrock.util;

namespace jabber.protocol
{
    /// <summary>
    /// Packets that have to/from information.
    /// </summary>
    [SVN(@"$Id: Packet.cs 733 2008-09-07 23:03:44Z hildjj $")]
    public class Packet : Element
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Packet(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(prefix, qname, doc)
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="localName"></param>
        /// <param name="doc"></param>
        public Packet(string localName, XmlDocument doc) :
            base(localName, URI.CLIENT, doc)
        {
        }

        /// <summary>
        /// The TO address
        /// </summary>
        public JID To
        {
            get { return (JID)this.GetAttr("to"); }
            set { SetAttr("to", value); }
        }

        /// <summary>
        ///  The FROM address
        /// </summary>
        public JID From
        {
            get { return (JID)this.GetAttr("from"); }
            set { SetAttr("from", value); }
        }

        /// <summary>
        /// The packet ID.
        /// </summary>
        public string ID
        {
            get { return this.GetAttr("id"); }
            set { this.SetAttr("id", value); }
        }

        /// <summary>
        /// Swap the To and the From addresses.
        /// </summary>
        public virtual void Swap()
        {
            string tmp = this.GetAttribute("to");
            this.SetAttribute("to", this.GetAttribute("from"));
            this.SetAttribute("from", tmp);
        }
    }
}
