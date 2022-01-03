using System.Globalization;
using System.Xml.Linq;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Data.Rooms;

public class SharpXmppRoom : IRoom
{
    private readonly IXmppClient client;

    private readonly object stateLock = new();
    private bool isJoined;
    private string? currentNickname;

    public SharpXmppRoom(IXmppClient client, Jid roomJid)
    {
        this.client = client;
        Jid = roomJid;

        SubscribeToEvents();
    }

    public void Dispose() => UnsubscribeFromEvents();

    private void SubscribeToEvents()
    {
        client.Presence += OnPresence;
        client.Message += OnMessage;
    }

    private void UnsubscribeFromEvents()
    {
        client.Presence -= OnPresence;
        client.Message -= OnMessage;
    }

    private void OnPresence(object _, IPresence presence)
    {
        if (presence.From?.Bare != Jid) return;
        if (presence.Error != null)
        {
            PresenceError?.Invoke(this, presence);
            return;
        }

        var xmlPresence = presence.Unwrap();
        var type = xmlPresence.Attribute("type")?.Value;
        var states = xmlPresence.Element(XNamespace.Get(Namespaces.MucUser) + Elements.X)
            ?.Elements(XNamespace.Get(Namespaces.MucUser) + Elements.Status);
        var statusCodes = states?.Select(
            s => (MucUserStatus)int.Parse(s.Attribute(Attributes.Code)?.Value!, CultureInfo.InvariantCulture))
            ?? Enumerable.Empty<MucUserStatus>();
        var isSelfPresence = statusCodes.Contains(MucUserStatus.SelfReferringPresence);

        if (ShouldFireJoinEvent())
            Joined?.Invoke(this, null);

        if (ShouldFireLeaveEvent())
            Left?.Invoke(this, presence);

        bool ShouldFireJoinEvent()
        {
            lock (stateLock)
            {
                if (isJoined || type == PresenceTypes.Unavailable || !isSelfPresence) return false;

                isJoined = true;
                return true;
            }
        }

        bool ShouldFireLeaveEvent() => isSelfPresence && type == PresenceTypes.Unavailable;
    }

    private void OnMessage(object sender, IMessage message)
    {
        if (message.From?.Bare != Jid) return;
        if (message.Subject != null)
        {
            SubjectChange?.Invoke(this, message);
            return;
        }
        if (message.From == JidWithNick)
        {
            SelfMessage?.Invoke(this, message);
            return;
        }
        if (message.From?.Resource == null)
        {
            AdminMessage?.Invoke(this, message);
            return;
        }

        RoomMessage?.Invoke(this, message);
    }

    public void Join(string? password = null)
    {
        throw new NotImplementedException();
    }

    public void Leave(string? reason)
    {
        throw new NotImplementedException();
    }

    public void SetNickname(string nickname)
    {
        lock (stateLock)
            currentNickname = nickname;
        client.SendPresence(new PresenceDetails { To = Jid.WithResource(nickname) });
    }

    public void SetSubject(string subject)
    {
        throw new NotImplementedException();
    }

    public event EventHandler? Joined;
    public event EventHandler<IPresence>? Left;
    public event EventHandler<IMessage>? SubjectChange;
    public event EventHandler<IPresence>? PresenceError;
    public event EventHandler<IMessage>? SelfMessage;
    public event EventHandler<IMessage>? AdminMessage;
    public event EventHandler<IMessage>? RoomMessage;
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
    private Jid JidWithNick => Jid.WithResource(currentNickname);

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
