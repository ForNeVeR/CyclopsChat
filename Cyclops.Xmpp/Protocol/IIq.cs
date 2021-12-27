using System.Xml;

namespace Cyclops.Xmpp.Protocol;

public interface IIq : IStanza
{
    public XmlElement? Error { get; }
}
