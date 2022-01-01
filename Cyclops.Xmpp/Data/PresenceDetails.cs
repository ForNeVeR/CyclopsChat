using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Data;

public sealed class PresenceDetails
{
    public PresenceType? Type { get; set; }
    public Jid? To { get; set; }
    public string? StatusText { get; set; }
    public StatusType? StatusType { get; set; }
    public string? PhotoHash { get; set; }
    public int? Priority { get; set; }
}
