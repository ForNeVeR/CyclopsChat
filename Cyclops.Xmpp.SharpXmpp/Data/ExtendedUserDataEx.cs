using System.Globalization;
using System.Xml.Linq;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Data;

internal static class ExtendedUserDataEx
{
    private class ExtendedUserData : IExtendedUserData
    {
        internal readonly XElement Element;
        public ExtendedUserData(XElement element)
        {
            Element = element;
            RoomItem = element.Elements(XNamespace.Get(Namespaces.MucUser) + Elements.Item)
                .SingleOrDefault()?.WrapAsRoomItem();
        }

        public IReadOnlyList<MucUserStatus?> Status => Element.Elements(XNamespace.Get(Namespaces.Muc) + "status")
            .Select(e => MapStatusCode(e.Attribute(Attributes.Code)?.Value))
            .ToList();

        public IRoomItem? RoomItem { get; }
    }

    private class XmppRoomItem : IRoomItem
    {
        private readonly XElement item;
        public XmppRoomItem(XElement item)
        {
            this.item = item;
        }

        public Jid? ActorJid
        {
            get
            {
                var actor = item.Element(XNamespace.Get(Namespaces.MucUser) + Elements.Actor);
                // TODO: clarify if such attribute exists; it was defined in Jabber-Net, and weren't found in XEP-0045.
                var jid = actor?.Attribute(Attributes.Jid)?.Value;
                if (jid == null) return null;
                return Jid.Parse(jid);
            }
        }

        public string? Reason => item.Element(XNamespace.Get(Namespaces.MucUser) + Elements.Reason)?.Value;
        public MucRole? Role => MapRole(item.Attribute(Attributes.Role)?.Value);
        public MucAffiliation? Affiliation => MapAffiliation(item.Attribute(Attributes.Affiliation)?.Value);
    }

    public static IExtendedUserData WrapAsUserData(this XElement x) => new ExtendedUserData(x);
    public static XElement Unwrap(this IExtendedUserData x) => ((ExtendedUserData)x).Element;

    private static IRoomItem WrapAsRoomItem(this XElement item) => new XmppRoomItem(item);

    private static MucUserStatus? MapStatusCode(string? code)
    {
        if (code == null) return null;
        if (int.TryParse(code, NumberStyles.Integer, CultureInfo.InvariantCulture, out var intCode))
        {
            return (MucUserStatus)intCode;
        }

        return null;
    }

    private static MucRole? MapRole(string? role) => role switch
    {
        MucRoles.Moderator => MucRole.Moderator,
        MucRoles.Participant => MucRole.Participant,
        MucRoles.Visitor => MucRole.Visitor,
        MucRoles.None => MucRole.None,
        { } => null,
        _ => throw new ArgumentException($"Unknown role: {role}.", nameof(role))
    };

    private static MucAffiliation? MapAffiliation(string? affiliation) => affiliation switch
    {
        MucAffiliations.Owner => MucAffiliation.Owner,
        MucAffiliations.Admin => MucAffiliation.Admin,
        MucAffiliations.Member => MucAffiliation.Member,
        MucAffiliations.None => MucAffiliation.None,
        MucAffiliations.Outcast => MucAffiliation.Outcast,
        { } => null,
        _ => throw new ArgumentException($"Unknown affiliation: {affiliation}.", nameof(affiliation))
    };
}
