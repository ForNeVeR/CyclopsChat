using System.Diagnostics.CodeAnalysis;
using System.Xml;
using jabber.protocol;

namespace Cyclops.Xmpp.JabberNet.Elements;

[SuppressMessage("ReSharper", "UnusedMember.Global")]
internal class PhotoX : Element
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

    public string? PhotoHash
    {
        get => GetElem("photo");
        set => SetElem("photo", value);
    }
}
