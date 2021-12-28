using Cyclops.Xmpp.Data;

namespace Cyclops.Xmpp.JabberNet.Data.Rooms;

internal static class AdminItemEx
{
    private class AdminItem : IAdminItem
    {
        private readonly jabber.protocol.iq.AdminItem adminItem;
        public AdminItem(jabber.protocol.iq.AdminItem adminItem)
        {
            this.adminItem = adminItem;
        }

        public string Nick => adminItem.Nick;
    }

    public static IAdminItem Map(this jabber.protocol.iq.AdminItem adminItem) => new AdminItem(adminItem);
}
