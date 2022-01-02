using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Client;

public interface IConferenceManager
{
    /// <summary>This status text should be added to any presence sent to any room.</summary>
    string? Status { set; }
    /// <summary>This status type should be added to any presence sent to any room.</summary>
    StatusType? StatusType { set; }

    IRoom GetRoom(Jid roomJid);
}
