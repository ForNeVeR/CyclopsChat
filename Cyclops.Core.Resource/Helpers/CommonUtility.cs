using System;
using System.Collections.Generic;
using System.Linq;
using Cyclops.Xmpp.Data;
using jabber.protocol;

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

        public static T GetNodeByName<T>(this IEnumerable<Element> node, string name) where T : Element
        {
            return node.FirstOrDefault<Element>(i => string.Equals(i.Name, name, StringComparison.InvariantCultureIgnoreCase)) as T;
        }
    }
}
