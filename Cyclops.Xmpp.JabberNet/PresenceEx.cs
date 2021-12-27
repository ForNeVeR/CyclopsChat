using System.Xml;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using jabber.protocol.client;

namespace Cyclops.Xmpp.JabberNet;

internal static class PresenceEx
{
    private class JabberNetPresence : IPresence
    {
        private readonly Presence jnPresence;

        public JabberNetPresence(Presence presence)
        {
            jnPresence = presence;
        }

        public XmlElement? this[string name] => jnPresence[name];
        public IEnumerable<XmlNode> Elements => jnPresence.Cast<XmlElement>();

        public IEntityIdentifier From => jnPresence.From;

        public string? Status => jnPresence.Status;
        public string? Show => jnPresence.Show;
    }

    public static IPresence Wrap(this Presence presence) => new JabberNetPresence(presence);
}
