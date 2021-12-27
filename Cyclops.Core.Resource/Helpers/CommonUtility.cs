using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using Cyclops.Xmpp.Data;

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

        public static T? GetNodeByName<T>(this IEnumerable<XmlElement> node, string name) where T : XmlElement =>
            node.FirstOrDefault(i => string.Equals(i.Name, name, StringComparison.InvariantCultureIgnoreCase)) as T;
    }
}
