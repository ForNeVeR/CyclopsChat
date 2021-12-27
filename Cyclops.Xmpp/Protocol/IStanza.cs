using System.Xml;

namespace Cyclops.Xmpp.Protocol;

public interface IStanza
{
    public XmlElement? this[string name] { get; }
    public IEnumerable<XmlNode> Elements { get; }

    public IEntityIdentifier From { get; }
}
