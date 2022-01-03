using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.JabberNet.Data.Rooms;

internal static class RoomItemEx
{
    private class RoomItem : IRoomItem
    {
        private jabber.protocol.iq.RoomItem roomItem;
        public RoomItem(jabber.protocol.iq.RoomItem roomItem)
        {
            this.roomItem = roomItem;
        }

        public Jid? ActorJid => roomItem.Actor?.JID?.Map();
        public string? Reason => roomItem.Reason;
        public MucRole? Role => roomItem.Role.Map();
        public MucAffiliation? Affiliation => roomItem.Affiliation.Map();
    }

    public static IRoomItem Map(this jabber.protocol.iq.RoomItem roomItem) => new RoomItem(roomItem);
}
