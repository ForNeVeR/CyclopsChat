using Cyclops.Xmpp.Data;
using jabber;

namespace Cyclops.Core.Resource
{
    public static class IdentifierBuilder
    {
        public static IEntityIdentifier Create(string user, string server, string resource)
        {
            return new JID(user, server, resource);
        }

        public static IEntityIdentifier Create(string str)
        {
            return new JID(str);
        }

        public static IEntityIdentifier WithAnotherResource(IEntityIdentifier sourceId, string resource)
        {
            return new JID(sourceId.User, sourceId.Server, resource);
        }
    }
}
