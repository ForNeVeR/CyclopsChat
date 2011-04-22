using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using jabber;
using jabber.protocol;
using jabber.protocol.x;

namespace Cyclops.Core.Resource.JabberNetExtensions
{
    //[XmlRoot(ElementName = "captcha", Namespace = "")]
    public class CaptchaAnswer : Element
    {
        public CaptchaAnswer(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        public CaptchaAnswer(string localName, XmlDocument doc)
            : base(localName, doc)
        {
        }

        public CaptchaAnswer(XmlDocument doc)
            : base("captcha", "urn:xmpp:captcha", doc)
        {
        }

        public CaptchaAnswer(string localName, string namespaceURI, XmlDocument doc)
            : base(localName, namespaceURI, doc)
        {
        }

        /// <summary>
        /// </summary>
        public CaptchaAnswerX CaptchaAnswerX
        {
            get { return GetChildElement<CaptchaAnswerX>(); }
            set { ReplaceChild<CaptchaAnswerX>(value); }
        }
    }

    public class CaptchaAnswerX : Element
    {
        public CaptchaAnswerX(XmlDocument doc)
            : base("x", "jabber:x:data", doc)
        {
        }

        public CaptchaAnswerX(string prefix, XmlQualifiedName qname, XmlDocument doc)
            : base(prefix, qname, doc)
        {
        }

        public CaptchaAnswerX(string localName, XmlDocument doc)
            : base(localName, doc)
        {
        }

        public CaptchaAnswerX(string localName, string namespaceURI, XmlDocument doc)
            : base(localName, namespaceURI, doc)
        {
        }

        private void AddField(string var, string value)
        {
            Field f = new Field(OwnerDocument);
            f.Var = var;
            f.Val = value;

            AddChild(f);
        }

        public void FillAnswer(string ocr, JID from, string challenge)
        {
            SetAttribute("type", "submit");
            AddField("FORM_TYPE", "urn:xmpp:captcha");
            AddField("from", from.ToString());
            AddField("challenge", challenge);
            AddField("sid", null);
            AddField("ocr", ocr);
        }
    }
}
