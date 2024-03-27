using System.Globalization;
using System.Xml.Linq;
using Cyclops.Core;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Client;
using Cyclops.Xmpp.SharpXmpp.Protocol;
using SharpXMPP.XMPP.Client.Elements;

namespace Cyclops.Xmpp.SharpXmpp.Data.Rooms;

public class SharpXmppRoom : IRoom
{
    private readonly ILogger logger;
    private readonly SharpXmppClient client;
    private readonly SharpXmppConferenceManager conferenceManager;

    private readonly object stateLock = new();
    private bool isJoined;
    private string? currentNickname;
    private readonly Dictionary<string, MucParticipant> participants = new();

    public SharpXmppRoom(ILogger logger, SharpXmppClient client, SharpXmppConferenceManager conferenceManager, Jid roomJid)
    {
        this.logger = logger;
        this.client = client;
        this.conferenceManager = conferenceManager;
        BareJid = roomJid.Bare;

        SubscribeToEvents();
    }

    public void Dispose()
    {
        conferenceManager.UnregisterRoom(this);
        UnsubscribeFromEvents();
    }

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

    private void OnPresence(object? _, IPresence presence)
    {
        if (presence.From?.Bare != BareJid) return;
        if (presence.Error != null)
        {
            logger.LogVerbose($"{BareJid}: error presence from {presence.From}.");
            PresenceError?.Invoke(this, presence);
            return;
        }

        var xmlPresence = presence.Unwrap();
        var type = xmlPresence.Attribute(Attributes.Type)?.Value;
        var states = xmlPresence.Element(XNamespace.Get(Namespaces.MucUser) + Elements.X)
            ?.Elements(XNamespace.Get(Namespaces.MucUser) + Elements.Status);
        var statusCodes = states?.Select(
            s => (MucUserStatus)int.Parse(s.Attribute(Attributes.Code)?.Value!, CultureInfo.InvariantCulture))
            ?? Enumerable.Empty<MucUserStatus>();
        var isSelfPresence = statusCodes.Contains(MucUserStatus.SelfReferringPresence);

        if (ShouldFireJoinEvent())
            Joined?.Invoke(this, EventArgs.Empty);

        if (ShouldFireLeaveEvent())
            Left?.Invoke(this, presence);

        ProcessParticipant(presence, type);

        bool ShouldFireJoinEvent()
        {
            lock (stateLock)
            {
                if (isJoined || type == PresenceTypes.Unavailable || !isSelfPresence) return false;

                isJoined = true;
                return true;
            }
        }

        bool ShouldFireLeaveEvent()
        {
            if (!isSelfPresence || type != PresenceTypes.Unavailable) return false;
            lock (stateLock)
            {
                isJoined = false;
                return true;
            }
        }
    }

    private void OnMessage(object? _, IMessage message)
    {
        if (message.From?.Bare != BareJid) return;
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

    private void ProcessParticipant(IPresence presence, string? type)
    {
        var nickname = presence.From?.Resource;
        if (nickname == null) return;

        lock (stateLock)
        {
            var existingParticipant = participants.TryGetValue(nickname, out var p) ? p : null;
            switch (type, existingParticipant)
            {
                case (PresenceTypes.Unavailable, { }):
                    participants.Remove(nickname);
                    ParticipantLeave?.Invoke(this, existingParticipant);
                    break;
                case (_, { }):
                    existingParticipant.UpdateFrom(presence);
                    ParticipantPresenceChange?.Invoke(this, existingParticipant);
                    break;
                case (_, null) when type != PresenceTypes.Unavailable:
                    var newParticipant = new MucParticipant(presence);
                    participants.Add(nickname, newParticipant);
                    ParticipantJoin?.Invoke(this, newParticipant);
                    break;
                case (PresenceTypes.Unavailable, null):
                    // Leaving notification from non-existing participant; ignore.
                    break;
            }
        }
    }

    private void SendRoomPresence()
    {
        var presence = new XMPPPresence
        {
            To = JidWithNick.Map()
        };
        presence.Add(new XElement(XNamespace.Get(SharpXMPP.Namespaces.MUC) + Elements.X));

        if (conferenceManager.Status != null)
        {
            presence.GetOrCreateChildElement(presence.Name.Namespace + Elements.Status)
                .Value = conferenceManager.Status;
        }

        if (conferenceManager.StatusType != null)
        {
            presence.GetOrCreateChildElement(presence.Name.Namespace + Elements.Show)
                .Value = conferenceManager.StatusType.Value.Map();
        }

        client.SendPresence(presence);
    }

    public void Join(string? password = null)
    {
        if (password != null)
            throw new NotImplementedException();

        SendRoomPresence();
    }

    public void Leave(string? reason) =>
        client.SendPresence(new PresenceDetails
        {
            To = BareJid,
            StatusText = reason,
            Type = PresenceType.Unavailable
        });

    public void SetNickname(string nickname)
    {
        var shouldSendPresence = false;
        lock (stateLock)
        {
            currentNickname = nickname;
            if (isJoined)
                shouldSendPresence = true;
        }

        if (shouldSendPresence)
            SendRoomPresence();
    }

    public void SetSubject(string subject)
    {
        var message = new XMPPMessage
        {
            To = BareJid.Map(),
        };
        message.GetOrCreateAttribute(Attributes.Type).Value = MessageType.GroupChat.Map();
        message.GetOrCreateChildElement(message.Name.Namespace + Elements.Subject).Value = subject;

        client.SendMessage(message);
    }

    public event EventHandler? Joined;
    public event EventHandler<IPresence>? Left;
    public event EventHandler<IMessage>? SubjectChange;
    public event EventHandler<IPresence>? PresenceError;
    public event EventHandler<IMessage>? SelfMessage;
    public event EventHandler<IMessage>? AdminMessage;
    public event EventHandler<IMessage>? RoomMessage;
    public event EventHandler<IMucParticipant>? ParticipantJoin;
    public event EventHandler<IMucParticipant>? ParticipantLeave;
    public event EventHandler<IMucParticipant>? ParticipantPresenceChange;

    public Jid BareJid { get; }
    private Jid JidWithNick => BareJid.WithResource(currentNickname);

    public IReadOnlyList<IMucParticipant> Participants
    {
        get
        {
            lock (stateLock)
                return participants.Values.ToList();
        }
    }

    public void SendPublicMessage(string body) => client.SendMessage(MessageType.GroupChat, BareJid, body);
}
