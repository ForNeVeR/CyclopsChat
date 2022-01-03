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
        RoomParticipantJid = presence.From!.Value;

        var extendedData = presence.Unwrap().Element(XNamespace.Get(Namespaces.MucUser) + Elements.X)?.WrapAsUserData();
        var roomItem = extendedData?.RoomItem;
        Role = roomItem?.Role;
        Affiliation = roomItem?.Affiliation;
    }

    public Jid RoomParticipantJid { get; }
    public Jid? RealJid => throw new NotImplementedException();
    public MucRole? Role { get; }
    public MucAffiliation? Affiliation { get; }
    public IPresence Presence { get; }
    public string Nick => throw new NotImplementedException();

    public void UpdateFrom(IPresence presence)
    {
        throw new NotImplementedException();
    }
}
