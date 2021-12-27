using System.Xml;
using Cyclops.Xmpp.Data;

namespace Cyclops.Xmpp.Client;

public interface IStanza
{
    public XmlElement? this[string name] { get; }
    public IEnumerable<XmlNode> Elements { get; }

    public IEntityIdentifier From { get; }
}
