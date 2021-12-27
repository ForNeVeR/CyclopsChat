using System.Linq;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.Resource.JabberNetExtensions
{
    public static class JabberCommonHelper
    {
        public static RoleChangedEventArgs? ConvertToRoleChangedEventArgs(
            IXmppDataExtractor extractor,
            IPresence presence)
        {
            var userX = extractor.GetExtendedUserData(presence);

            if (userX?.ActorJid == null || (presence.Status == null && presence.Show == null))
                return null;

            Role role = Role.Regular;
            if (!userX.Status.IsNullOrEmpty())
            {
                if (userX.Status.Contains(MucUserStatus.Kicked))
                    role = Role.Kicked;
                if (userX.Status.Contains(MucUserStatus.Banned))
                    role = Role.Banned;
            }
            else
                role = ConvertRole(userX.Role, userX.Affiliation);

            return new RoleChangedEventArgs { To = presence.From?.Resource, Role = role };
        }

        public static Role ConvertRole(MucRole? role, MucAffiliation? affiliation)
        {
            if (affiliation == MucAffiliation.Outcast)
                return Role.Banned;
            if (role == MucRole.Visitor)
                return Role.Devoiced;
            if (affiliation == MucAffiliation.Owner)
                return Role.Owner;
            if (affiliation == MucAffiliation.Admin)
                return Role.Admin;
            if (role == MucRole.Moderator)
                return Role.Moder;
            if (affiliation == MucAffiliation.Member)
                return Role.Member;
            return Role.Regular;
        }
    }
}
