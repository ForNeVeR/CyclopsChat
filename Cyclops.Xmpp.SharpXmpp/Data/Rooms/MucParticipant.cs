using System.Xml.Linq;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Data.Rooms;

internal class MucParticipant : IMucParticipant
{
    public MucParticipant(IPresence presence)
    {
        Presence = presence;
        var jidWithNick = presence.From!.Value;

        RoomParticipantJid = jidWithNick;
        Nick = jidWithNick.Resource!;

        UpdateFrom(presence);
    }

    public Jid RoomParticipantJid { get; }
    public Jid? RealJid => throw new NotImplementedException();
    public MucRole? Role { get; private set; }
    public MucAffiliation? Affiliation { get; private set; }
    public IPresence Presence { get; }
    public string Nick { get; }

    public void UpdateFrom(IPresence presence)
    {
        var extendedData = presence.Unwrap().Element(XNamespace.Get(Namespaces.MucUser) + Elements.X)?.WrapAsUserData();
        var roomItem = extendedData?.RoomItem;
        Role = roomItem?.Role;
        Affiliation = roomItem?.Affiliation;
    }
}
