namespace Cyclops.Xmpp.Protocol;

public interface IDiscoNode
{
    Jid? Jid { get; }
    string? Node { get; }
    string? Name { get; }
    IEnumerable<IDiscoNode> Children { get; }
}
