using Cyclops.Xmpp.Protocol;
using SharpXMPP.XMPP;

namespace Cyclops.Xmpp.SharpXmpp.Protocol;

internal static class JidEx
{
    public static Jid Map(this JID jid) => new(jid.User, jid.Domain, jid.Resource);
}
