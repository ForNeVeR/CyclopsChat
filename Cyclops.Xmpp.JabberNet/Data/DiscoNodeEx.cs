using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.JabberNet.Data;

internal static class DiscoNodeEx
{
    private class DiscoNode : IDiscoNode
    {
        private readonly jabber.connection.DiscoNode discoNode;
        public DiscoNode(jabber.connection.DiscoNode discoNode)
        {
            this.discoNode = discoNode;
        }

        public Jid Jid => discoNode.JID.Map();
        public string Node => discoNode.Node;
        public string Name => discoNode.Name;
        public IEnumerable<IDiscoNode> Children => discoNode.Children.Cast<jabber.connection.DiscoNode>()
            .Select(dn => dn.Wrap());
    }

    public static IDiscoNode Wrap(this jabber.connection.DiscoNode discoNode) => new DiscoNode(discoNode);
}
