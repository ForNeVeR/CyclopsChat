using Cyclops.Xmpp.Protocol;
using jabber.protocol.client;

namespace Cyclops.Xmpp.JabberNet.Protocol;

internal static class PresenceEx
{
    private class JabberNetPresence : Stanza, IPresence
    {
        private readonly Presence presence;
        public JabberNetPresence(Presence presence) : base(presence)
        {
            this.presence = presence;
        }

        public string? Status => presence.Status;
        public string? Show => presence.Show;

        public IError Error => presence.Error.Wrap();
    }

    public static IPresence Wrap(this Presence presence) => new JabberNetPresence(presence);
}
