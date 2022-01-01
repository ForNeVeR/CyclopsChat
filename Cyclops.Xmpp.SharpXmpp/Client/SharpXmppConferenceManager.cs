using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;

namespace Cyclops.Xmpp.SharpXmpp.Client;

public class SharpXmppConferenceManager : IConferenceManager
{
    public string? Status { set => throw new NotImplementedException(); }
    public StatusType? StatusType { set => throw new NotImplementedException(); }
}
