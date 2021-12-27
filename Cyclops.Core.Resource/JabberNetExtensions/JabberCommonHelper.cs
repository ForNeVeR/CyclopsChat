using System.Linq;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Xmpp.Client;
using jabber;
using jabber.connection;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Cyclops.Core.Resource.JabberNetExtensions
{
    public static class JabberCommonHelper
    {
        public static void GetClientVersionAsnyc(UserSession session, JID nickJid, IqCB callback)
        {
            var versionIq = new VersionIQ(session.JabberClient.Document) {To = nickJid, Type = IQType.get};
            session.ConferenceManager.BeginIQ(versionIq, callback, null);
        }

        public static RoleChangedEventArgs ConvertToRoleChangedEventArgs(IPresence presence, RoomParticipant participant)
        {
            var userX = presence["x"] as UserX;
            if (userX == null || userX.RoomItem == null)
                return null;

            if (userX.RoomItem.Actor == null || (presence.Status == null && presence.Show == null))
                return null;

            Role role = Role.Regular;
            if (!userX.Status.IsNullOrEmpty())
            {
                if (userX.Status[0] == RoomStatus.KICKED)
                    role = Role.Kicked;
                if (userX.Status[0] == RoomStatus.BANNED)
                    role = Role.Banned;
            }
            else
                role = ConvertRole(userX.RoomItem.Role, userX.RoomItem.Affiliation);

            return new RoleChangedEventArgs { To = presence.From.Resource, Role = role };
        }

        public static Role ConvertRole(RoomRole role, RoomAffiliation affiliation)
        {
            if (affiliation == RoomAffiliation.outcast)
                return Role.Banned;
            if (role == RoomRole.visitor)
                return Core.Role.Devoiced;
            if (affiliation == RoomAffiliation.owner)
                return Core.Role.Owner;
            if (affiliation == RoomAffiliation.admin)
                return Core.Role.Admin;
            if (role == RoomRole.moderator)
                return Core.Role.Moder;
            if (affiliation == RoomAffiliation.member)
                return Core.Role.Member;
            return Core.Role.Regular;
        }

        public static RoomParticipant FindParticipant(this ParticipantCollection collection, JID nickJid)
        {
            return collection.OfType<RoomParticipant>().FirstOrDefault(i => i.NickJID.Equals(nickJid));
        }
    }
}
