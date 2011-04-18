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

namespace jabber.protocol.client
{
    /// <summary>
    /// Message type attribute
    /// </summary>
    [SVN(@"$Id: Message.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public enum MessageType
    {
        /// <summary>
        /// Normal message
        /// </summary>
        normal = -1,
        /// <summary>
        /// Error message
        /// </summary>
        error,
        /// <summary>
        /// Chat (one-to-one) message
        /// </summary>
        chat,
        /// <summary>
        /// Groupchat
        /// </summary>
        groupchat,
        /// <summary>
        /// Headline
        /// </summary>
        headline
    }
    /// <summary>
    /// A client-to-client message.
    /// TODO: Some XHTML is supported by setting the .Html property,
    /// but extra xmlns="" get put everywhere at the moment.
    /// </summary>
    [SVN(@"$Id: Message.cs 724 2008-08-06 18:09:25Z hildjj $")]
    public class Message : Packet
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="doc"></param>
        public Message(XmlDocument doc) : base("message", doc)
        {
            ID = NextID();
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="qname"></param>
        /// <param name="doc"></param>
        public Message(string prefix, XmlQualifiedName qname, XmlDocument doc) :
            base(qname.Name, doc)  // Note:  *NOT* base(prefix, qname, doc), so that xpath matches are easier
        {
        }

        /// <summary>
        /// The message type attribute
        /// </summary>
        public MessageType Type
        {
            get { return (MessageType) GetEnumAttr("type", typeof(MessageType)); }
            set
            {
                if (value == MessageType.normal)
                    RemoveAttribute("type");
                else
                    SetAttribute("type", value.ToString());
            }
        }

        private void NormalizeHtml(XmlElement body, string html)
        {
            XmlDocument d = new XmlDocument();
            d.LoadXml("<html xmlns='" + URI.XHTML + "'>" + html + "</html>");
            foreach (XmlNode node in d.DocumentElement.ChildNodes)
            {
                body.AppendChild(this.OwnerDocument.ImportNode(node, true));
            }
        }

        /// <summary>
        /// On set, creates both an html element, and a body element, which will
        /// have the de-html'd version of the html element.
        /// </summary>
        public string Html
        {
            get
            {
                // Thanks, Mr. Postel.
                XmlElement h = this["html"];
                if (h == null)
                    return "";
                XmlElement b = h["body"];
                if (b == null)
                    return "";
                string xml = b.InnerXml;
                // HACK: yeah, yeah, I know.
                return xml.Replace(" xmlns=\"" + URI.XHTML + "\"", "");
            }
            set
            {
                XmlElement html = GetOrCreateElement("html", URI.XHTML_IM, null);
                XmlElement body = html["body", URI.XHTML];
                if (body == null)
                {
                    body =  this.OwnerDocument.CreateElement(null, "body", URI.XHTML);
                    html.AppendChild(body);
                }
                else
                    body.RemoveAll();
                NormalizeHtml(body, value);
                this.Body = body.InnerText;
            }
        }

        /// <summary>
        /// The message body
        /// </summary>
        public string Body
        {
            get { return GetElem("body"); }
            set { SetElem("body", value); }
        }

        /// <summary>
        /// The message thread
        /// TODO: some help to generate these, please.
        /// </summary>
        public string Thread
        {
            get { return GetElem("thread"); }
            set { SetElem("thread", value); }
        }
        /// <summary>
        /// The message subject
        /// </summary>
        public string Subject
        {
            get { return GetElem("subject"); }
            set { SetElem("subject", value); }
        }
        /// <summary>
        /// The first x tag, regardless of namespace.
        /// </summary>
        [Obsolete("This almost certainly doesn't do what you want.")]
        public XmlElement X
        {
            get { return this["x"]; }
            set { this.AddChild(value); }
        }

        /// <summary>
        /// Message error.
        /// </summary>
        public Error Error
        {
            get { return GetChildElement<Error>(); }
            set
            {
                this.Type = MessageType.error;
                ReplaceChild<Error>(value);
            }
        }
    }
}
