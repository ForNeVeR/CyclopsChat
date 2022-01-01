using System.Diagnostics.CodeAnalysis;
using System.Xml;
using jabber;
using jabber.protocol;
using jabber.protocol.x;

namespace Cyclops.Xmpp.JabberNet.Elements;

[SuppressMessage("ReSharper", "ClassNeverInstantiated.Global")]
internal class CaptchaAnswer : Element
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

    public CaptchaAnswer(string localName, string namespaceUri, XmlDocument doc)
        : base(localName, namespaceUri, doc)
    {
    }

    /// <summary>
    /// </summary>
    public CaptchaAnswerX? CaptchaAnswerX
    {
        get => GetChildElement<CaptchaAnswerX>();
        set => ReplaceChild(value);
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

    public CaptchaAnswerX(string localName, string namespaceUri, XmlDocument doc)
        : base(localName, namespaceUri, doc)
    {
    }

    private void AddField(string var, string? value)
    {
        AddChild(new Field(OwnerDocument!)
        {
            Var = var,
            Val = value
        });
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
