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
            isNickConflictMode = false;
            if (!e.Success) return;

            if (room != null)
            {
                UnSubscribeToEvents();
                Leave("Replaced with new connection.");
            }

            room = session.ConferenceManager.GetRoom((JID)ConferenceId);
            room.Nickname = ConferenceId.Resource;
            SubscribeToEvents();
            BeginJoin(this, EventArgs.Empty);
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
            room.OnRoomConfig += room_OnRoomConfig;
            room.OnParticipantJoin += room_OnParticipantJoin;
            room.OnParticipantLeave += room_OnParticipantLeave;

            session.JabberClient.OnPresence += JabberClient_OnPresence;
        }

        private IQ room_OnRoomConfig(Room room, IQ parent)
        {
            return parent;
        }

        void JabberClient_OnPresence(object sender, Presence pres)
        {
            if (!pres.From.BareJID.Equals(((JID)ConferenceId).BareJID) && !pres.From.Equals(Session.CurrentUserId))
                return;

            var roleChangedEventArgs = JabberCommonHelper.ConvertToRoleChangedEventArgs(pres, room.Participants.FindParticipant(pres.From));
            if (roleChangedEventArgs != null && IsInConference)
                RoleChanged(this, roleChangedEventArgs);
            
            IEntityIdentifier from = pres.From.Equals(Session.CurrentUserId) ? ConferenceId : pres.From;

            var member = Members.FirstOrDefault(i => i.ConferenceUserId.Equals(from));
            if (member == null)
                return;


            bool successHandling = ((AvatarsManager)AvatarsManager).ProcessAvatarChangeHash(pres, ConferenceId);
            if (!member.IsSubscribed)
            {
                if (!successHandling)
                    AvatarsManager.SendAvatarRequest(from);
                ((ConferenceMember)member).IsSubscribed = true;
            }

        }

        private void UnSubscribeToEvents()
        {
            session.JabberClient.OnPresence -= JabberClient_OnPresence;

            if (room == null)
                return;

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
        
        private bool waitingForPassword = false;  
        private void room_OnPresenceError(Room room, Presence pres)
        {
            if (pres.Error == null)
                return;

            if (captchaMode || (waitingForPassword && pres.Error.Code != 401))
                return;

            switch (pres.Error.Code)
            {
                case 409: //conflict
                    if (!isNickConflictMode)
                    {
                        isNickConflictMode = true;
                        Joined(this, new ConferenceJoinEventArgs(ConferenceJoinErrorKind.NickConflict,
                                                                 ErrorMessageResources.NickConflictErrorMessage));
                    }
                    break;

                case 403: //banned
                    Joined(this, new ConferenceJoinEventArgs(ConferenceJoinErrorKind.Banned,
                                                             ErrorMessageResources.BannedErrorMessage));
                    break;

                case 401:
                    ConferenceJoinErrorKind error;
                    if (waitingForPassword)
                        error = ConferenceJoinErrorKind.PasswordRequired;
                    else
                        error = ConferenceJoinErrorKind.PasswordRequired;

                    waitingForPassword = true;
                    Joined(this, new ConferenceJoinEventArgs(error, pres.Error.Message));
                    break;

                case 407:
                    AccessDenied(this, EventArgs.Empty);
                    break;

                case 503:
                    ServiceUnavailable(this, EventArgs.Empty);
                    break;

                case 405://i.e. changing nickname if one is visitor and this action is not allowed at the conference
                    MethodNotAllowedError(this, EventArgs.Empty);
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
            if (waitingForPassword)
                waitingForPassword = false;
            
            Joined(this, new ConferenceJoinEventArgs());
            foreach (RoomParticipant participant in room.Participants)
            {
                if (!Members.Any(i => (JID) i.ConferenceUserId == participant.NickJID))
                    Members.AsInternalImpl().Add(new ConferenceMember(session, this, participant, room)
                                                     {
                                                         AvatarUrl = AvatarsManager.GetFromCache(string.Empty)
                                                     });
            }
            IsInConference = true;
        }

        private void room_OnParticipantLeave(Room room, RoomParticipant participant)
        {
            //HANDLE NICK CHANGE
            bool nickChangeAction = false;
            string newNick = string.Empty;

            var memberObj = Members.FirstOrDefault(i => i.Nick == participant.Nick);
            if (memberObj == null)
                return;

            var nickChange = participant.Presence.OfType<UserX>().FirstOrDefault(i => i.Status != null && i.Status.Any(s => s == RoomStatus.NEW_NICK));
            if (nickChange != null && nickChange.OfType<AdminItem>().Any(i => !string.IsNullOrEmpty(i.Nick)))
            {
                newNick = nickChange.OfType<AdminItem>().First(i => !string.IsNullOrEmpty(i.Nick)).Nick;
                string oldNick = participant.Nick;
                NickChange(this, new NickChangeEventArgs(oldNick, newNick));
                nickChangeAction = true;
            }
            
            if (nickChangeAction && participant.NickJID.Equals(ConferenceId))
            {
                // a small hack:
                room.RetrieveListByAffiliation(participant.Affiliation, (r, p, s) =>
                    {
                        var newParticipantObj = r.Participants.OfType<RoomParticipant>().FirstOrDefault(i => i.Nick == newNick);
                        if (newParticipantObj == null)
                        {
                            //LOG?
                        }
                        else
                            Members.AsInternalImpl().Add(new ConferenceMember(session, this, newParticipantObj, r)
                                {
                                    AvatarUrl = memberObj != null ? memberObj.AvatarUrl : null
                                });

                    }, new Object());
                ConferenceId = IdentifierBuilder.WithAnotherResource(ConferenceId, newNick);
            }
            //HANDLE NICK CHANGE (END)

            if (!nickChangeAction)
                ParticipantLeave(this, new ConferenceMemberEventArgs(memberObj));
           
            Members.AsInternalImpl().Remove(i => participant.NickJID == (JID) i.ConferenceUserId);
        }

        private void room_OnParticipantJoin(Room room, RoomParticipant participant)
        {
            if (!Members.Any(i => (JID)i.ConferenceUserId == participant.NickJID))
            {
                var member = new ConferenceMember(session, this, participant, room)
                                 {
                                     AvatarUrl = AvatarsManager.GetFromCache(string.Empty)
                                 };
                Members.AsInternalImpl().Add(member);
                if (IsInConference) //hack :)
                    ParticipantJoin(this, new ConferenceMemberEventArgs(member));
            }
        }
         
        private void room_OnRoomMessage(object sender, Message msg)
        {
            if (string.IsNullOrEmpty(msg.Body)) return;

            DateTime stamp = DateTime.Now;
            var delay = msg.OfType<Delay>().FirstOrDefault();
            if (delay != null && delay.Stamp != DateTime.MinValue)
                stamp = delay.Stamp;


            Messages.AsInternalImpl().Add(new ConferenceUserMessage(session, sender as Room, msg) { Timestamp = stamp });
        }

        public event EventHandler<CaptchaEventArgs> CaptchaRequirment = delegate { };

        private bool captchaMode = false;
        private string captchaChallenge = null;

        private void room_OnAdminMessage(object sender, Message msg)
        {
            // CAPTCHA required
            BitmapImage captcha = null;
            if (CaptchaHelper.ExtractImage(msg, ref captcha, ref captchaChallenge))
            {
                CaptchaRequirment(this, new CaptchaEventArgs(captcha));
                captchaMode = true;
                return;
            }

            //Not allowed to change subject
            if (msg.Error != null && !string.IsNullOrEmpty(msg.Subject))
            {
                CantChangeSubject(this, EventArgs.Empty);
                return;
            }

            //not allowed to send messages
            if (msg.Error != null && msg.Error.Code == 403)
            {
                MethodNotAllowedError(this, EventArgs.Empty);
                return;
            }

            //unknown:
            Messages.AsInternalImpl().Add(new ConferenceUserMessage(session, sender as Room, msg));
        }

        private void room_OnSelfMessage(object sender, Message msg)
        {
            if (string.IsNullOrEmpty(msg.Body)) return;
            Messages.AsInternalImpl().Add(new ConferenceUserMessage(session, msg, true));
        }

        private void room_OnSubjectChange(object sender, Message msg)
        {
            if (msg.From != null && !string.IsNullOrEmpty(msg.From.Resource))
                SubjectChanged(this, new SubjectChangedEventArgs(msg.From.Resource, msg.Subject));

            Subject = msg.Subject;
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

        private IEntityIdentifier conferenceId;
        public IEntityIdentifier ConferenceId
        {
            get { return conferenceId; }
            private set
            {
                conferenceId = value;
                OnPropertyChanged("ConferenceId");
            }
        }

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

        public void ChangeSubject(string subj)
        {
            room.Subject = subj;
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

            if (waitingForPassword)
            {                
                //let's rejoin
                room.Leave("");
                room.Join(body);
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

        private bool isNickConflictMode = false;

        public void RejoinWithNewNick(string nick)
        {
            ConferenceId = IdentifierBuilder.WithAnotherResource(ConferenceId, nick);
            if (session.IsAuthenticated)
                Authenticated(this, new AuthenticationEventArgs());
        }

        internal void RaiseSomebodyChangedHisStatusEvent(IConferenceMember member)
        {
            SomebodyChangedHisStatus(this, new ConferenceMemberEventArgs(member));
        }

        public event EventHandler<RoleChangedEventArgs> RoleChanged = delegate { }; 
        public event EventHandler<ConferenceMemberEventArgs> SomebodyChangedHisStatus = delegate { }; 
        public event EventHandler ServiceUnavailable = delegate { };
        public event EventHandler<ConferenceJoinEventArgs> Joined = delegate { };
        public event EventHandler<KickedEventArgs> Kicked = delegate { };
        public event EventHandler<BannedEventArgs> Banned = delegate { };
        public event EventHandler InvalidCaptchaCode = delegate { };
        public event EventHandler AccessDenied = delegate { };
        public event EventHandler<NickChangeEventArgs> NickChange = delegate { };
        public event EventHandler BeginJoin = delegate { };
        public event EventHandler StartReconnectTimer = delegate { };
        public event EventHandler CantChangeSubject = delegate { };
        public event EventHandler MethodNotAllowedError = delegate { };
        public event EventHandler<SubjectChangedEventArgs> SubjectChanged = delegate { };
        public event EventHandler<ConferenceMemberEventArgs> ParticipantLeave = delegate { };
        public event EventHandler<ConferenceMemberEventArgs> ParticipantJoin = delegate { };

        #endregion

    }
}