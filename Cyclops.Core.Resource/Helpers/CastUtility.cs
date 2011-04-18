using jabber.connection;
using jabber.protocol.iq;

namespace Cyclops.Core.Resource
{
    internal static class CommonUtility
    {
        internal static InternalObservableCollection<T> AsInternalImpl<T>(this IObservableCollection<T> collection)
        {
            return collection as InternalObservableCollection<T>;
        }

        internal static bool IsModer(this RoomParticipant participant)
        {
            return participant.Role == RoomRole.moderator ||
                    participant.Affiliation == RoomAffiliation.admin ||
                    participant.Affiliation == RoomAffiliation.owner;
        }
    }
}