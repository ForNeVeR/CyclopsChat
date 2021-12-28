using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.JabberNet.Data.Rooms;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;
using jabber.connection;

namespace Cyclops.Xmpp.JabberNet.Data;

internal static class RoomParticipantEx
{
    private class MucParticipant : IMucParticipant
    {
        private readonly RoomParticipant participant;

        public MucParticipant(RoomParticipant participant)
        {
            this.participant = participant;
        }

        public IEntityIdentifier RoomParticipantJid => participant.NickJID;
        public IEntityIdentifier? RealJid => participant.RealJID;
        public MucRole? Role => participant.Role.Map();
        public MucAffiliation? Affiliation => participant.Affiliation.Map();
        public IPresence Presence => participant.Presence.Wrap();
        public string Nick => participant.Nick;
    }

    public static IMucParticipant Wrap(this RoomParticipant participant) => new MucParticipant(participant);
}
