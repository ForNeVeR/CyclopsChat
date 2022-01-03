using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Data.Rooms;

internal class MucParticipant : IMucParticipant
{
    public Jid RoomParticipantJid => throw new NotImplementedException();
    public Jid? RealJid => throw new NotImplementedException();
    public MucRole? Role => throw new NotImplementedException();
    public MucAffiliation? Affiliation => throw new NotImplementedException();
    public IPresence Presence => throw new NotImplementedException();
    public string Nick => throw new NotImplementedException();

    public void UpdateFrom(IPresence presence)
    {
        throw new NotImplementedException();
    }
}
