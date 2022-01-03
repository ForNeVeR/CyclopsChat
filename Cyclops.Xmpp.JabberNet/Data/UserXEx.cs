using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.JabberNet.Data.Rooms;
using jabber.protocol.iq;

namespace Cyclops.Xmpp.JabberNet.Data;

internal static class UserXEx
{
    private class ExtendedUserData : IExtendedUserData
    {
        internal readonly UserX UserX;
        public ExtendedUserData(UserX userX)
        {
            UserX = userX;
        }

        public IReadOnlyList<MucUserStatus?> Status => UserX.Status.Select(x => x.Map()).ToList();
        public IRoomItem? RoomItem => UserX.RoomItem?.Map();
    }

    public static IExtendedUserData Wrap(this UserX userX) => new ExtendedUserData(userX);
    public static UserX Unwrap(this IExtendedUserData userData) => ((ExtendedUserData)userData).UserX;
}
