using System.Xml;

namespace Cyclops.Xmpp.Protocol;

public interface IStanza
{
    XmlElement? this[string name] { get; }
    IEnumerable<XmlNode> Nodes { get; }

    Jid? From { get; }
    Jid? To { get; }
}
