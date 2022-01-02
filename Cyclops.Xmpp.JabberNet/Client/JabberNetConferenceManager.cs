using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.JabberNet.Data.Rooms;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;
using jabber.connection;

namespace Cyclops.Xmpp.JabberNet.Client;

internal class JabberNetConferenceManager : IConferenceManager
{
    private readonly ConferenceManager conferenceManager;
    public JabberNetConferenceManager(ConferenceManager conferenceManager)
    {
        this.conferenceManager = conferenceManager;
        conferenceManager.BeforeRoomPresenceOut += (_, e) =>
        {
            var pres = e.RoomPresence;
            if (Status != null)
                pres.Status = Status;
            if (StatusType != null)
                pres.Show = StatusType?.Map();
        };
    }

    public string? Status { get; set; }
    public StatusType? StatusType { get; set; }

    public IRoom GetRoom(Jid roomJid) => conferenceManager.GetRoom(roomJid.Map()).Wrap();
}
