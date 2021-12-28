using System.Xml;
using Cyclops.Xmpp.Data.Rooms;

namespace Cyclops.Xmpp.Data;

public interface IExtendedUserData
{
    IEnumerable<XmlNode> Nodes { get; }

    public IReadOnlyList<MucUserStatus?> Status { get; }
    public IRoomItem? RoomItem { get; }
}
