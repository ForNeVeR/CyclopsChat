using System.Xml.Linq;

namespace Cyclops.Xmpp.SharpXmpp.Protocol;

internal static class XElementEx
{
    public static XElement GetOrCreateChildElement(this XContainer parent, XName name)
    {
        var child = parent.Element(name);
        if (child != null) return child;

        child = new XElement(name);
        parent.Add(child);

        return child;
    }

    public static XAttribute GetOrCreateAttribute(this XElement element, XName name)
    {
        var attribute = element.Attribute(name);
        if (attribute != null) return attribute;

        attribute = new XAttribute(name, "");
        element.Add(attribute);

        return attribute;
    }
}
