using Cyclops.Xmpp.Protocol;
using SharpXMPP.XMPP.Client.Elements;

namespace Cyclops.Xmpp.SharpXmpp.Protocol;

internal static class PresenceEx
{
    private class Presence : IPresence
    {
        private readonly XMPPPresence presence;
        public Presence(XMPPPresence presence)
        {
            this.presence = presence;
        }

        public Jid? From => presence.From.Map();
        public Jid? To => presence.To.Map();
        public string? Status => throw new NotImplementedException();
        public string? Show => throw new NotImplementedException();
        public IError? Error => throw new NotImplementedException();
    }

    public static IPresence Wrap(this XMPPPresence presence) => new Presence(presence);
}
