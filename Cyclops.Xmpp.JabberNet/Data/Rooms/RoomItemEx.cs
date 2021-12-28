using Cyclops.Xmpp.Data.Rooms;
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

        public IEntityIdentifier? ActorJid => roomItem.Actor?.JID;
        public string Reason => roomItem.Reason;
        public MucRole? Role => roomItem.Role.Map();
        public MucAffiliation? Affiliation => roomItem.Affiliation.Map();
    }

    public static IRoomItem Map(this jabber.protocol.iq.RoomItem roomItem) => new RoomItem(roomItem);
}
