using System.Xml.Linq;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Data;

public class DiscoNode : IDiscoNode
{
    public DiscoNode(Jid? jid, string? node, string? name, IEnumerable<XElement> items)
    {
        Jid = jid;
        Node = node;
        Name = name;
        Children = items.Select(Wrap).ToList();
    }

    public Jid? Jid { get; }
    public string? Node { get; }
    public string? Name { get; }
    public IEnumerable<IDiscoNode> Children { get; }

    private static IDiscoNode Wrap(XElement item)
    {
        var jid = item.Attribute(Attributes.Jid)?.Value;
        var node = item.Attribute(Attributes.Node)?.Value;
        var name = item.Attribute(Attributes.Name)?.Value;
        return new DiscoNode(
            jid == null ? null : Xmpp.Protocol.Jid.Parse(jid),
            node,
            name,
            Enumerable.Empty<XElement>());
    }
}
