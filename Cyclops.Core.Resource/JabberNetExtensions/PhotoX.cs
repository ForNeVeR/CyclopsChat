using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using jabber.protocol;

namespace Cyclops.Core.Resource.JabberNetExtensions
{
    public class PhotoX : Element
    {
        public PhotoX(XmlDocument doc) : base("x", "vcard-temp:x:update", doc)
        {
        }

        public PhotoX(string prefix, XmlQualifiedName qname, XmlDocument doc) : base(prefix, qname, doc)
        {
        }

        public PhotoX(string localName, XmlDocument doc) : base(localName, doc)
        {
        }

        public PhotoX(string localName, string namespaceUri, XmlDocument doc) : base(localName, namespaceUri, doc)
        {
        }

        /// <summary>
        /// </summary>
        public string PhotoHash
        {
            get { return GetElem("photo"); }
            set { SetElem("photo", value); }
        }
    }
}
