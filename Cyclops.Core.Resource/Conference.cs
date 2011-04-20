using System;
using System.Linq;
using Cyclops.Core.Avatars;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Resource.Avatars;
using Cyclops.Core.Resources;
using jabber;
using jabber.connection;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Cyclops.Core.Resource
{
    public class Conference : NotifyPropertyChangedBase, IConference
    {
        private readonly UserSession session;
        private Room room;

        internal Conference(UserSession session, IEntityIdentifier conferenceId)
        {
            this.session = session;
            ConferenceId = conferenceId;

            session.ConnectionDropped += OnConnectionDropped;
            Members = new InternalObservableCollection<IConferenceMember>();
            Messages = new InternalObservableCollection<IConferenceMessage>();
            AvatarsManager = new AvatarsManager(session);
            AvatarsManager.AvatarChange += AvatarsManagerAvatarChange;

            //if we are currently authenticated lets join to the channel imidiatly
            if (session.IsAuthenticated)
                Authenticated(this, new AuthenticationEventArgs());

            session.Authenticated += Authenticated;
        }

        private void AvatarsManagerAvatarChange(object sender, AvatarChangedEventArgs e)
        {
            var member = Members.FirstOrDefault(i => i.ConferenceUserId.Equals(e.UserId));
            if (member != null)
                ((ConferenceMember) member).AvatarUrl = e.BitmapImage;
        }

        #region IConference Members

        public event EventHandler<DisconnectEventArgs> Disconnected = delegate { };

        #endregion

        private void OnConnectionDropped(object sender, AuthenticationEventArgs e)
        {
            Members.AsInternalImpl().Clear();
            IsInConference = false;
            Disconnected(this, new DisconnectEventArgs(ConnectionErrorKind.ConnectionError, e.ErrorMessage));
            Messages.AsInternalImpl().Add(new SystemConferenceMessage
                                              {
                                                  Body = "Reconnect in a 10 seconds ..."  //TODO: MOVE to presentation layer
                                              });
        }

        private void Authenticated(object sender, AuthenticationEventArgs e)
        {
            if (!e.Success) return;

            if (room != null)
            {
                UnSubscribeToEvents();
                Leave("Replaced with new connection.");
            }

            
            Messages.AsInternalImpl().Add(new SystemConferenceMessage { Body = "Entering the room..." }); //TODO: MOVE to presentation layer
            room = session.ConferenceManager.GetRoom((JID)ConferenceId);
            SubscribeToEvents();
            room.Join();
        }

        private void SubscribeToEvents()
        {
            room.OnJoin += room_OnJoin;
            room.OnLeave += room_OnLeave;
            room.OnSubjectChange += room_OnSubjectChange;
            room.OnPresenceError += room_OnPresenceError;
            room.OnSelfMessage += room_OnSelfMessage;
            room.OnAdminMessage += room_OnAdminMessage;
            room.OnRoomMessage += room_OnRoomMessage;
            room.OnParticipantJoin += room_OnParticipantJoin;
            room.OnParticipantLeave += room_OnParticipantLeave;
        }

        private void UnSubscribeToEvents()
        {
            room.OnJoin -= room_OnJoin;
            room.OnLeave -= room_OnLeave;
            room.OnSubjectChange -= room_OnSubjectChange;
            room.OnPresenceError -= room_OnPresenceError;
            
            room.OnSelfMessage -= room_OnSelfMessage;
            room.OnAdminMessage -= room_OnAdminMessage;
            room.OnRoomMessage -= room_OnRoomMessage;
            room.OnParticipantJoin -= room_OnParticipantJoin;
            room.OnParticipantLeave -= room_OnParticipantLeave;
        }

        private void room_OnPresenceError(Room room, Presence pres)
        {
            if (pres.Error == null)
                return;

            switch (pres.Error.Code)
            {
                case 409: //conflict
                    Joined(this, new ConferenceJoinEventArgs(ConferenceJoinErrorKind.NickConflict,
                                                             ErrorMessageResources.NickConflictErrorMessage));
                    break;

                case 403: //banned
                    Joined(this, new ConferenceJoinEventArgs(ConferenceJoinErrorKind.Banned,
                                                             ErrorMessageResources.BannedErrorMessage));
                    break;

                case 401: //not-auth
                    Joined(this, new ConferenceJoinEventArgs(ConferenceJoinErrorKind.PasswordRequired,
                                                             ErrorMessageResources.PasswordRequiredErrorMessage));
                    break;

                    //captcha... :(
            }
        }

        private void room_OnLeave(Room room, Presence pres)
        {
            if (!pres.IsNullOrEmpty() && pres["x"] != null)
            {
                var userX = pres["x"] as UserX;
                if (!userX.Status.IsNullOrEmpty())
                {
                    if (userX.Status.Any(i => i == RoomStatus.KICKED))
                        Kicked(this, new KickedEventArgs(null, userX.RoomItem.Reason));
                    else if (userX.Status.Any(i => i == RoomStatus.BANNED))
                        Banned(this, new BannedEventArgs(null, userX.RoomItem.Reason));
                }
            }

            Members.AsInternalImpl().Clear();
            IsInConference = false;
        }

        private void room_OnJoin(Room room)
        {
            Joined(this, new ConferenceJoinEventArgs());
            IsInConference = true;
            RoomParticipant meAsParticipant = null;
            foreach (RoomParticipant participant in room.Participants)
            {
                if (!Members.Any(i => (JID) i.ConferenceUserId == participant.NickJID))
                    Members.AsInternalImpl().Add(new ConferenceMember(session, participant, room));
            }
        }

        private void room_OnParticipantLeave(Room room, RoomParticipant participant)
        {
            Members.AsInternalImpl().Remove(i => participant.NickJID == (JID) i.ConferenceUserId);
        }

        private void room_OnParticipantJoin(Room room, RoomParticipant participant)
        {
            AvatarsManager.SendAvatarRequest(participant.NickJID);
            if (!Members.Any(i => (JID) i.ConferenceUserId == participant.NickJID))
                Members.AsInternalImpl().Add(new ConferenceMember(session, participant, room) { AvatarUrl = AvatarsManager.GetFromCache(participant.NickJID)});
        }

        private void room_OnRoomMessage(object sender, Message msg)
        {
            Messages.AsInternalImpl().Add(new ConferenceUserMessage(session, sender as Room, msg));
        }

        public event EventHandler<CaptchaEventArgs> CaptchaRequirment = delegate { }; 

        private void room_OnAdminMessage(object sender, Message msg)
        {
            if (msg.OfType<Element>().Any(i => string.Equals(i.Name, "captcha", StringComparison.InvariantCultureIgnoreCase)))
            {
                var element = msg.OfType<Element>().FirstOrDefault(i => string.Equals(i.Name, "x", StringComparison.InvariantCultureIgnoreCase)) as OOB;
                if (element != null)
                {
                    string captcha = element.Url;
                    CaptchaRequirment(this, new CaptchaEventArgs(captcha));
                    return;
                }
                
            }

            Messages.AsInternalImpl().Add(new ConferenceUserMessage(session, sender as Room, msg));
        }

        private void room_OnSelfMessage(object sender, Message msg)
        {
            Messages.AsInternalImpl().Add(new ConferenceUserMessage(session, msg, true));
        }

        private void room_OnSubjectChange(object sender, Message msg)
        {
            Subject = msg.Body;
        }

        #region Implementation of ISessionHolder

        public IUserSession Session
        {
            get { return session; }
        }

        #endregion

        #region Implementation of IConference

        private bool isInConference;
        private string subject;

        public string Subject
        {
            get { return subject; }
            private set
            {
                subject = value;
                OnPropertyChanged("Subject");
            }
        }

        public bool IsInConference
        {
            get { return isInConference; }
            private set
            {
                isInConference = value;
                OnPropertyChanged("IsInConference");
            }
        }

        public IAvatarsManager AvatarsManager { get; private set; }

        public IEntityIdentifier ConferenceId { get; private set; }

        public IObservableCollection<IConferenceMember> Members { get; private set; }

        public IObservableCollection<IConferenceMessage> Messages { get; private set; }

        public void Leave(string reason = "")
        {
            try
            {
                IsInConference = false;
                room.Leave(reason);
            }
            catch
            {
            }
        }

        public void LeaveAndClose(string reason = "")
        {
            Leave();
            UnSubscribeToEvents();
            room = null;

            Session.Conferences.AsInternalImpl().Remove(this);
        }

        public void SendPublicMessage(string body)
        {
            if (room.IsParticipating)
                room.PublicMessage(body);
        }

        public void SendPrivateMessage(IEntityIdentifier target, string body)
        {
            if (room.IsParticipating)
                room.PrivateMessage(target.Resource, body);
        }

        public event EventHandler<ConferenceJoinEventArgs> Joined = delegate { };
        public event EventHandler<KickedEventArgs> Kicked = delegate { };
        public event EventHandler<BannedEventArgs> Banned = delegate { };

        #endregion
    }
}