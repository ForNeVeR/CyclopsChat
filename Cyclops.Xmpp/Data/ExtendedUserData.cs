using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Data;

public class ExtendedUserData
{
    public IEntityIdentifier? ActorJid { get; }
    public IReadOnlyList<MucUserStatus?> Status { get; }
    public MucRole? Role { get; }
    public MucAffiliation? Affiliation { get; }

    public ExtendedUserData(IEntityIdentifier? actorJid, IReadOnlyList<MucUserStatus?> status, MucRole? role, MucAffiliation? affiliation)
    {
        ActorJid = actorJid;
        Status = status;
        Role = role;
        Affiliation = affiliation;
    }
}
