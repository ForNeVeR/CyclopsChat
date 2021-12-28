using System.Xml;

namespace Cyclops.Xmpp.Protocol;

public interface IStanza
{
    XmlElement? this[string name] { get; }
    IEnumerable<XmlNode> Nodes { get; }

    IEntityIdentifier? From { get; }
    IEntityIdentifier? To { get; }
}
