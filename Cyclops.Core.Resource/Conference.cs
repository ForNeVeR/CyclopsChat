using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Media.Imaging;
using Cyclops.Core.Avatars;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Resource.Avatars;
using Cyclops.Core.Resource.JabberNetExtensions;
using Cyclops.Core.Resources;
using jabber;
using jabber.connection;
using jabber.protocol;
using jabber.protocol.client;
using jabber.protocol.iq;
using jabber.protocol.x;

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

            StartReconnectTimer(this, EventArgs.Empty);
        }

        private void Authenticated(object sender, AuthenticationEventArgs e)
        {
            if (!e.Success) return;

            if (room != null)
            {
                UnSubscribeToEvents();
                Leave("Replaced with new connection.");
            }

            BeginJoin(this, EventArgs.Empty);
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

            session.JabberClient.OnPresence += JabberClient_OnPresence;
        }

        void JabberClient_OnPresence(object sender, Presence pres)
        {
            if (!pres.From.BareJID.Equals(((JID)ConferenceId).BareJID))
                return;

            var member = Members.FirstOrDefault(i => i.ConferenceUserId.Equals(pres.From));
            if (member == null)
                return;


            bool successHandling = ((AvatarsManager)AvatarsManager).ProcessAvatarChangeHash(pres);
            if (!member.IsSubscribed)
            {
                if (!successHandling)
                    AvatarsManager.SendAvatarRequest(pres.From);
                ((ConferenceMember)member).IsSubscribed = true;
            }

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

            session.JabberClient.OnPresence -= JabberClient_OnPresence;
        }

        private void room_OnPresenceError(Room room, Presence pres)
        {
            if (pres.Error == null)
                return;

            if (captchaMode)
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

                case 407:
                    AccessDenied(this, EventArgs.Empty);
                    break;

#if DEBUG
                default:
                    Debugger.Break();
                break;
#endif
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
            foreach (RoomParticipant participant in room.Participants)
            {
                if (!Members.Any(i => (JID) i.ConferenceUserId == participant.NickJID))
                    Members.AsInternalImpl().Add(new ConferenceMember(session, participant, room));
            }
        }

        private string changeNickMode = null;
        private void room_OnParticipantLeave(Room room, RoomParticipant participant)
        {
            //if leave for nick changing

            //handle nick change
            var nickChange = participant.Presence.OfType<UserX>().FirstOrDefault(i => i.Status != null && i.Status.Any(s => s == RoomStatus.NEW_NICK));
            if (nickChange != null && nickChange.OfType<AdminItem>().Any(i => !string.IsNullOrEmpty(i.Nick)))
            {
                string newNick = nickChange.OfType<AdminItem>().First(i => !string.IsNullOrEmpty(i.Nick)).Nick;
                string oldNick = participant.Nick;
                NickChange(this, new NickChangeEventArgs(oldNick, newNick));
                changeNickMode = newNick;
            }
            
            Members.AsInternalImpl().Remove(i => participant.NickJID == (JID) i.ConferenceUserId);

            if (participant.NickJID.Equals(ConferenceId))
            {
                //TODO:
            }
        }

        private void room_OnParticipantJoin(Room room, RoomParticipant participant)
        {
            if (!Members.Any(i => (JID) i.ConferenceUserId == participant.NickJID))
                Members.AsInternalImpl().Add(
                    new ConferenceMember(session, participant, room)
                        {
                            AvatarUrl = AvatarsManager.GetFromCache(participant.NickJID)
                        });
        }
         
        private void room_OnRoomMessage(object sender, Message msg)
        {
            Messages.AsInternalImpl().Add(new ConferenceUserMessage(session, sender as Room, msg));
        }

        public event EventHandler<CaptchaEventArgs> CaptchaRequirment = delegate { };

        private bool captchaMode = false;
        private string captchaChallenge = null;

        private void room_OnAdminMessage(object sender, Message msg)
        {
            BitmapImage captcha = null;
            if (CaptchaHelper.ExtractImage(msg, ref captcha, ref captchaChallenge))
            {
                CaptchaRequirment(this, new CaptchaEventArgs(captcha));
                captchaMode = true;
                return;
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
            if (captchaMode)
            {
                ConferenceManager manager = session.ConferenceManager;
                var iq = new TypedIQ<CaptchaAnswer>(session.JabberClient.Document);
                iq.To = ((JID) ConferenceId).BareJID;
                iq.Type = IQType.set;
                iq.Instruction.CaptchaAnswerX = new CaptchaAnswerX(session.JabberClient.Document);
                iq.Instruction.CaptchaAnswerX.FillAnswer(body, (JID)ConferenceId, captchaChallenge);
                manager.BeginIQ(iq, OnCaptchaResponse, new Object());
                return;
            }
                
            room.PublicMessage(body);
        }

        public bool ChangeNick(string value)
        {
            if (IsInConference)
            {
                //TODO: GLOBAL VALIDATOR!!!
                if (!string.IsNullOrWhiteSpace(value) && value.Length < 30 &&
                    !value.Contains("@") && !value.Contains("/"))
                    room.Nickname = value;
            }

            return true;
        }

        private void OnCaptchaResponse(object sender, IQ iq, object data)
        {
            if (iq.Error == null)
                captchaMode = false;
            else
            {
                //let's rejoin
                room.Leave("");
                room.Join();
                InvalidCaptchaCode(this, EventArgs.Empty);
            }
        }

        public void SendPrivateMessage(IEntityIdentifier target, string body)
        {
            room.PrivateMessage(target.Resource, body);
        }

        public event EventHandler<ConferenceJoinEventArgs> Joined = delegate { };
        public event EventHandler<KickedEventArgs> Kicked = delegate { };
        public event EventHandler<BannedEventArgs> Banned = delegate { };
        public event EventHandler InvalidCaptchaCode = delegate { };
        public event EventHandler AccessDenied = delegate { };
        public event EventHandler<NickChangeEventArgs> NickChange = delegate { };
        public event EventHandler BeginJoin = delegate { };
        public event EventHandler StartReconnectTimer = delegate { };

        #endregion

    }
}