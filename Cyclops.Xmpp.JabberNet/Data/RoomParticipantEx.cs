using Cyclops.Xmpp.Data;
using jabber.connection;
using jabber.protocol.iq;

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

        public MucRole? Role => participant.Role switch
        {
            RoomRole.moderator => MucRole.Moderator,
            RoomRole.participant => MucRole.Participant,
            RoomRole.visitor => MucRole.Visitor,
            RoomRole.none => MucRole.None,
            _ => null
        };

        public MucAffiliation? Affiliation => participant.Affiliation switch
        {
            RoomAffiliation.admin => MucAffiliation.Admin,
            RoomAffiliation.member => MucAffiliation.Member,
            RoomAffiliation.none => MucAffiliation.None,
            RoomAffiliation.outcast => MucAffiliation.Outcast,
            RoomAffiliation.owner => MucAffiliation.Owner,
            _ => null
        };
    }

    public static IMucParticipant Wrap(this RoomParticipant participant) => new MucParticipant(participant);
}
