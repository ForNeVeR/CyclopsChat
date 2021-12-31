using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Data;

public class PresenceDetails
{
    public Jid? To { get; set; }
    public string? StatusText { get; set; }
    public StatusType? StatusType { get; set; }
    public string? PhotoHash { get; set; }
}
