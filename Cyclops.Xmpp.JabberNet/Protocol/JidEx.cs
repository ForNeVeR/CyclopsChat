using Cyclops.Xmpp.Protocol;
using jabber;

namespace Cyclops.Xmpp.JabberNet.Protocol;

public static class JidEx
{
    public static Jid Map(this JID jid) => new(jid.User, jid.Server, jid.Resource);
    public static JID Map(this Jid jid) => new(jid.Local, jid.Domain, jid.Resource);
}
