using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Client;

public class SharpXmppConferenceManager : IConferenceManager
{
    public string? Status { set => throw new NotImplementedException(); }
    public StatusType? StatusType { set => throw new NotImplementedException(); }

    public IRoom GetRoom(Jid roomJid)
    {
        throw new NotImplementedException();
    }
}
