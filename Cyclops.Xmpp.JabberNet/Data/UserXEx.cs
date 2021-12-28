using System.Xml;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.JabberNet.Data.Rooms;
using jabber.protocol.iq;

namespace Cyclops.Xmpp.JabberNet.Data;

internal static class UserXEx
{
    private class ExtendedUserData : IExtendedUserData
    {
        private readonly UserX userX;
        public ExtendedUserData(UserX userX)
        {
            this.userX = userX;
        }

        public IEnumerable<XmlNode> Nodes => userX.Cast<XmlNode>();
        public IReadOnlyList<MucUserStatus?> Status => userX.Status.Select(x => x.Map()).ToList();
        public IRoomItem? RoomItem => userX.RoomItem?.Map();
    }

    public static IExtendedUserData Wrap(this UserX userX) => new ExtendedUserData(userX);
}
