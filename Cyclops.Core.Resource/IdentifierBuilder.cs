using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;
using jabber;

namespace Cyclops.Core.Resource
{
    public static class IdentifierBuilder
    {
        public static Jid Create(string user, string server, string resource)
        {
            return new Jid(user, server, resource);
        }

        public static Jid Create(string str)
        {
            return new JID(str).Map();
        }

        public static Jid WithAnotherResource(Jid sourceId, string resource)
        {
            return new Jid(sourceId.User, sourceId.Server, resource);
        }
    }
}
