using Cyclops.Xmpp.Data;

namespace Cyclops.Xmpp.Client;

public interface IConferenceManager
{
    string? Status { get; set; }
    StatusType? StatusType { get; set; }
}
