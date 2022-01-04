using Cyclops.Core;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Data.Rooms;

namespace Cyclops.Xmpp.SharpXmpp.Client;

public class SharpXmppConferenceManager : IConferenceManager
{
    private readonly ILogger logger;
    private readonly SharpXmppClient client;

    private readonly object stateLock = new();
    private readonly Dictionary<Jid, IRoom> rooms = new();

    public SharpXmppConferenceManager(ILogger logger, SharpXmppClient client)
    {
        this.logger = logger;
        this.client = client;
    }

    public string? Status { set => throw new NotImplementedException(); }
    public StatusType? StatusType { set => throw new NotImplementedException(); }

    public IRoom GetRoom(Jid roomJid)
    {
        lock (stateLock)
        {
            if (!rooms.TryGetValue(roomJid.Bare, out var room))
            {
                room = new SharpXmppRoom(logger, client, this, roomJid.Bare);
                rooms.Add(roomJid, room);
            }

            return room;
        }
    }

    public void UnregisterRoom(IRoom room)
    {
        lock (stateLock)
            rooms.Remove(room.BareJid);
    }
}
