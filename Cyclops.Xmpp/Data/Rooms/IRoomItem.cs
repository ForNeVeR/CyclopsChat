using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Data.Rooms;

public interface IRoomItem
{
    public Jid? ActorJid { get; }
    public string Reason { get; }
    public MucRole? Role { get; }
    public MucAffiliation? Affiliation { get; }
}
