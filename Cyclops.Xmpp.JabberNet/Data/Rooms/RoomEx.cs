using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;
using jabber.connection;
using jabber.protocol.client;

namespace Cyclops.Xmpp.JabberNet.Data.Rooms;

internal static class RoomEx
{
    private class Room : IRoom
    {
        private readonly jabber.connection.Room room;
        public Room(jabber.connection.Room room)
        {
            this.room = room;

            SubscribeToEvents();
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

        private void SubscribeToEvents()
        {
            room.OnJoin += OnJoin;
            room.OnLeave += OnLeave;
            room.OnSubjectChange += OnSubjectChange;
            room.OnPresenceError += OnPresenceError;
            room.OnSelfMessage += OnSelfMessage;
            room.OnAdminMessage += OnAdminMessage;
            room.OnRoomMessage += OnRoomMessage;
            room.OnParticipantJoin += OnParticipantJoin;
            room.OnParticipantLeave += OnParticipantLeave;
            room.OnParticipantPresenceChange += OnParticipantPresenceChange;
        }

        public void Dispose()
        {
            room.OnJoin -= OnJoin;
            room.OnLeave -= OnLeave;
            room.OnSubjectChange -= OnSubjectChange;
            room.OnPresenceError -= OnPresenceError;
            room.OnSelfMessage -= OnSelfMessage;
            room.OnAdminMessage -= OnAdminMessage;
            room.OnRoomMessage -= OnRoomMessage;
            room.OnParticipantJoin -= OnParticipantJoin;
            room.OnParticipantLeave -= OnParticipantLeave;
            room.OnParticipantPresenceChange -= OnParticipantPresenceChange;
        }

        public void Join(string? password = null) => room.Join(password);
        public void Leave(string? reason) => room.Leave(reason);

        public void SetNickname(string nickname)
        {
            room.Nickname = nickname;
        }

        public void SetSubject(string subject)
        {
            room.Subject = subject;
        }

        public Jid BareJid => room.JID.Map();

        public IReadOnlyList<IMucParticipant> Participants =>
            room.Participants.Cast<RoomParticipant>().Select(r => r.Wrap()).ToList();

        public Task<IReadOnlyList<IMucParticipant>> GetParticipants(MucAffiliation? participantAffiliation)
        {
            var task = new TaskCompletionSource<IReadOnlyList<IMucParticipant>>();
            room.RetrieveListByAffiliation(
                participantAffiliation.Map(),
                (_, participants, _) =>
                {
                    try
                    {
                        var mapped = participants.Cast<RoomParticipant>().Select(rp => rp.Wrap()).ToList();
                        task.SetResult(mapped);
                    }
                    catch (Exception ex)
                    {
                        task.SetException(ex);
                    }
                },
                null);

            return task.Task;
        }

        public void SendPublicMessage(string body) => room.PublicMessage(body);

        private void OnJoin(jabber.connection.Room _) => Joined?.Invoke(this, null);
        private void OnLeave(jabber.connection.Room _, Presence presence) => Left?.Invoke(this, presence.Wrap());
        private void OnSubjectChange(object _, Message message) => SubjectChange?.Invoke(this, message.Wrap());
        private void OnPresenceError(jabber.connection.Room _, Presence presence) =>
            PresenceError?.Invoke(this, presence.Wrap());
        private void OnSelfMessage(object _, Message message) => SelfMessage?.Invoke(this, message.Wrap());
        private void OnAdminMessage(object _, Message message) => AdminMessage?.Invoke(this, message.Wrap());
        private void OnRoomMessage(object _, Message message) => RoomMessage?.Invoke(this, message.Wrap());
        private void OnParticipantJoin(jabber.connection.Room _, RoomParticipant participant) =>
            ParticipantJoin?.Invoke(this, participant.Wrap());
        private void OnParticipantLeave(jabber.connection.Room _, RoomParticipant participant) =>
            ParticipantLeave?.Invoke(this, participant.Wrap());
        private void OnParticipantPresenceChange(jabber.connection.Room _, RoomParticipant participant) =>
            ParticipantPresenceChange?.Invoke(this, participant.Wrap());
    }

    public static IRoom Wrap(this jabber.connection.Room room) => new Room(room);
}
