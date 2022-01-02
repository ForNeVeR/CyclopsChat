using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Data.Rooms;

public class SharpXmppRoom : IRoom
{
    private readonly IXmppClient client;
    public SharpXmppRoom(IXmppClient client, Jid roomJid)
    {
        this.client = client;
        Jid = roomJid;
    }

    public void Dispose()
    {
    }

    public void Join(string? password = null)
    {
        throw new NotImplementedException();
    }

    public void Leave(string? reason)
    {
        throw new NotImplementedException();
    }

    public void SetNickname(string nickname) =>
        client.SendPresence(new PresenceDetails { To = Jid.WithResource(nickname) });

    public void SetSubject(string subject)
    {
        throw new NotImplementedException();
    }

    public event EventHandler? Joined
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<IPresence>? Left
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<IMessage>? SubjectChange
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<IPresence>? PresenceError
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<IMessage>? SelfMessage
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<IMessage>? AdminMessage
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<IMessage>? RoomMessage
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<IMucParticipant>? ParticipantJoin
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<IMucParticipant>? ParticipantLeave
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<IMucParticipant>? ParticipantPresenceChange
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public Jid Jid { get; }
    public IReadOnlyList<IMucParticipant> Participants => throw new NotImplementedException();
    public Task<IReadOnlyList<IMucParticipant>> GetParticipants(MucAffiliation? participantAffiliation)
    {
        throw new NotImplementedException();
    }

    public void SendPublicMessage(string body)
    {
        throw new NotImplementedException();
    }

    public void SendPrivateMessage(string nick, string body)
    {
        throw new NotImplementedException();
    }
}
