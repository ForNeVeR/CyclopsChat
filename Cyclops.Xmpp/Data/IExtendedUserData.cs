using Cyclops.Xmpp.Data.Rooms;

namespace Cyclops.Xmpp.Data;

public interface IExtendedUserData
{
    public IReadOnlyList<MucUserStatus?> Status { get; }
    public IRoomItem? RoomItem { get; }
}
