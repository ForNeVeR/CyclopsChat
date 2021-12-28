using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Data.Rooms;

public interface IMucParticipant
{
    /// <summary>room@server/nickname</summary>
    IEntityIdentifier RoomParticipantJid { get; }
    IEntityIdentifier? RealJid { get; }
    MucRole? Role { get; }
    MucAffiliation? Affiliation { get; }
    IPresence Presence { get; }
    string Nick { get; }
}
