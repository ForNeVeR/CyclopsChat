using Cyclops.Xmpp.Protocol;
using jabber.protocol.client;

namespace Cyclops.Xmpp.JabberNet.Protocol;

internal static class PresenceEx
{
    private class JabberNetPresence : Stanza, IPresence
    {
        public readonly Presence Presence;
        public JabberNetPresence(Presence presence) : base(presence)
        {
            Presence = presence;
        }

        public string? Status => Presence.Status;
        public string? Show => Presence.Show;

        public IError Error => Presence.Error.Wrap();
    }

    public static IPresence Wrap(this Presence presence) => new JabberNetPresence(presence);
    public static Presence Unwrap(this IPresence presence) => ((JabberNetPresence)presence).Presence;
}
