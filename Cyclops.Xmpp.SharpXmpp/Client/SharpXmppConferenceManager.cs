using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Data.Rooms;

namespace Cyclops.Xmpp.SharpXmpp.Client;

public class SharpXmppConferenceManager : IConferenceManager
{
    private readonly SharpXmppClient client;
    public SharpXmppConferenceManager(SharpXmppClient client)
    {
        this.client = client;
    }

    public string? Status { set => throw new NotImplementedException(); }
    public StatusType? StatusType { set => throw new NotImplementedException(); }

    public IRoom GetRoom(Jid roomJid) => new SharpXmppRoom(client, roomJid);
}
