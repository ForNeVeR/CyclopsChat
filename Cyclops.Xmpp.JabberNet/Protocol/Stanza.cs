using System.Xml;
using Cyclops.Xmpp.Protocol;
using jabber.protocol;

namespace Cyclops.Xmpp.JabberNet.Protocol;

internal abstract class Stanza : IStanza
{
    private readonly Packet packet;
    protected Stanza(Packet packet)
    {
        this.packet = packet;
    }

    public XmlElement? this[string name] => packet[name];
    public IEnumerable<XmlNode> Nodes => packet.Cast<XmlNode>();

    public IEntityIdentifier? From => packet.From;
    public IEntityIdentifier? To => packet.To;
}
