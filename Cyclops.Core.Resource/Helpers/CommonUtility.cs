using System;
using System.Collections.Generic;
using System.Linq;
using jabber.connection;
using jabber.protocol;
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
        
        public static T GetNodeByName<T>(this IEnumerable<Element> node, string name) where T : Element
        {
            return node.FirstOrDefault<Element>(i => string.Equals(i.Name, name, StringComparison.InvariantCultureIgnoreCase)) as T;
        }
    }
}