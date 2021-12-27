using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.JabberNet.Helpers;
using jabber.connection;

namespace Cyclops.Xmpp.JabberNet.Data;

public static class RoomParticipantEx
{
    private class MucParticipant : IMucParticipant
    {
        private readonly RoomParticipant participant;

        public MucParticipant(RoomParticipant participant)
        {
            this.participant = participant;
        }

        public MucRole? Role => participant.Role.Map();
        public MucAffiliation? Affiliation => participant.Affiliation.Map();
    }

    public static IMucParticipant Wrap(this RoomParticipant participant) => new MucParticipant(participant);
}
