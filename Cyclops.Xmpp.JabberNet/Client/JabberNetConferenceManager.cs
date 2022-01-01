using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using jabber.connection;

namespace Cyclops.Xmpp.JabberNet.Client;

public class JabberNetConferenceManager : IConferenceManager
{
    public JabberNetConferenceManager(ConferenceManager conferenceManager)
    {
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
}
