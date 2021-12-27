namespace Cyclops.Xmpp.Data;

public interface IMucParticipant
{
    MucRole? Role { get; }
    MucAffiliation? Affiliation { get; }
}
