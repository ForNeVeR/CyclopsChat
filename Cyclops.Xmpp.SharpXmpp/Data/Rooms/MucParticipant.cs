using System.Xml.Linq;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Data.Rooms;

internal class MucParticipant : IMucParticipant
{
    public MucParticipant(IPresence presence)
    {
        Presence = presence; // technically redundant since UpdateFrom will set this anyway
        var jidWithNick = presence.From!.Value;

        RoomParticipantJid = jidWithNick;
        Nick = jidWithNick.Resource!;

        UpdateFrom(presence);
    }

    public Jid RoomParticipantJid { get; }

    public Jid? RealJid
    {
        get
        {
            var item = Presence.Unwrap().Element(XNamespace.Get(Namespaces.MucUser) + Elements.X)?
                .Element(XNamespace.Get(Namespaces.MucUser) + Elements.Item);

            var jid = item?.Attribute(Attributes.Jid)?.Value;
            return jid == null ? null : Jid.Parse(jid);
        }
    }
    public MucRole? Role { get; private set; }
    public MucAffiliation? Affiliation { get; private set; }
    public IPresence Presence { get; set; }
    public string Nick { get; }

    public void UpdateFrom(IPresence presence)
    {
        Presence = presence;

        var extendedData = presence.Unwrap().Element(XNamespace.Get(Namespaces.MucUser) + Elements.X)?.WrapAsUserData();
        var roomItem = extendedData?.RoomItem;
        Role = roomItem?.Role;
        Affiliation = roomItem?.Affiliation;
    }
}
