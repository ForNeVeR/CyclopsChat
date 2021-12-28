using Cyclops.Xmpp.Data.Rooms;

namespace Cyclops.Core.Resource
{
    internal static class CommonUtility
    {
        internal static InternalObservableCollection<T> AsInternalImpl<T>(this IObservableCollection<T> collection)
        {
            return collection as InternalObservableCollection<T>;
        }

        internal static bool IsModer(this IMucParticipant participant)
        {
            return participant.Role == MucRole.Moderator ||
                   participant.Affiliation is MucAffiliation.Admin or MucAffiliation.Owner;
        }
    }
}
