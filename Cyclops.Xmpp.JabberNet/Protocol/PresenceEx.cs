using Cyclops.Xmpp.Protocol;
using jabber.protocol.client;

namespace Cyclops.Xmpp.JabberNet.Protocol;

internal static class PresenceEx
{
    private class JabberNetPresence : Stanza, IPresence
    {
        private readonly Presence jnPresence;
        public JabberNetPresence(Presence presence) : base(presence)
        {
            jnPresence = presence;
        }

        public string? Status => jnPresence.Status;
        public string? Show => jnPresence.Show;
    }

    public static IPresence Wrap(this Presence presence) => new JabberNetPresence(presence);
}
