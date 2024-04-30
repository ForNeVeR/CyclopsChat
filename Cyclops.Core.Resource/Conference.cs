using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;
using Cyclops.Core.Avatars;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Helpers;
using Cyclops.Core.Resource.Avatars;
using Cyclops.Core.Resource.Helpers;
using Cyclops.Core.Resource.JabberNetExtensions;
using Cyclops.Core.Resources;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.Resource;

public class Conference : NotifyPropertyChangedBase, IConference
{
    private readonly ILogger logger;
    private readonly Dispatcher dispatcher;
    private readonly IXmppDataExtractor dataExtractor;
    private readonly UserSession session;
    private IRoom? room;
    private object _stateLock = new();

    internal Conference(
        ILogger logger,
        Dispatcher dispatcher,
        UserSession session,
        IXmppDataExtractor dataExtractor,
        Jid conferenceId)
    {
        this.logger = logger;
        this.dispatcher = dispatcher;
        this.session = session;
        this.dataExtractor = dataExtractor;
        ConferenceId = conferenceId;

        session.ConnectionDropped += OnConnectionDropped;
        Members = new InternalObservableCollection<IConferenceMember>();
        Messages = new InternalObservableCollection<IConferenceMessage>();
        AvatarsManager = new AvatarsManager(logger, session, dataExtractor);
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
            room.Dispose();
        }

        room = session.GetRoom(ConferenceId);
        room.SetNickname(ConferenceId.Resource);
        SubscribeToEvents();
        BeginJoin(this, EventArgs.Empty);
        room.Join();
    }

    private void SubscribeToEvents()
    {
        room!.Joined += room_OnJoin;
        room.Left += room_OnLeave;
        room.SubjectChange += room_OnSubjectChange;
        room.PresenceError += room_OnPresenceError;
        room.SelfMessage += room_OnSelfMessage;
        room.AdminMessage += room_OnAdminMessage;
        room.RoomMessage += room_OnRoomMessage;
        room.ParticipantJoin += room_OnParticipantJoin;
        room.ParticipantLeave += room_OnParticipantLeave;

        session.Presence += OnPresence;
    }

    private void OnPresence(object sender, IPresence pres)
    {
        var presenceFrom = pres.From;
        if (presenceFrom?.Bare != ConferenceId.Bare && presenceFrom != Session.CurrentUserId)
            return;

        var roleChangedEventArgs = JabberCommonHelper.ConvertToRoleChangedEventArgs(dataExtractor, pres);
        if (roleChangedEventArgs != null && IsInConference)
            RoleChanged(this, roleChangedEventArgs);

        var from = pres.From.Equals(Session.CurrentUserId) ? ConferenceId : pres.From;

        var member = Members.FirstOrDefault(i => i.ConferenceUserId.Equals(from));
        if (member == null)
            return;

        ProcessAvatar(pres, member);
    }

    private void UnSubscribeToEvents()
    {
        session.Presence -= OnPresence;

        if (room == null)
            return;

        room.Joined -= room_OnJoin;
        room.Left -= room_OnLeave;
        room.SubjectChange -= room_OnSubjectChange;
        room.PresenceError -= room_OnPresenceError;
        room.SelfMessage -= room_OnSelfMessage;
        room.AdminMessage -= room_OnAdminMessage;
        room.RoomMessage -= room_OnRoomMessage;
        room.ParticipantJoin -= room_OnParticipantJoin;
        room.ParticipantLeave -= room_OnParticipantLeave;
    }

    private bool waitingForPassword = false;
    private void room_OnPresenceError(object _, IPresence pres)
    {
        dispatcher.InvokeAsyncIfRequired(() =>
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

                        var eventArgs = new ConferenceJoinEventArgs(
                            ConferenceJoinErrorKind.NickConflict,
                            ErrorMessageResources.NickConflictErrorMessage,
                            pres);

                        Joined(this, eventArgs);
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

                case 405
                    : //i.e. changing nickname if one is visitor and this action is not allowed at the conference
                    MethodNotAllowedError(this, EventArgs.Empty);
                    break;

                case 404:
                    NotFound(this, EventArgs.Empty);
                    break;
#if DEBUG
                default:
                    Debugger.Break();
                    break;
#endif
            }
        });
    }

    private void room_OnLeave(object _, IPresence pres)
    {
        dispatcher.InvokeAsyncIfRequired(() =>
        {
            var userX = dataExtractor.GetExtendedUserData(pres);
            if (userX?.Status.IsNullOrEmpty() == false)
            {
                if (userX.Status.Any(i => i == MucUserStatus.Kicked))
                    Kicked(this, new KickedEventArgs(null, userX.RoomItem?.Reason));
                else if (userX.Status.Any(i => i == MucUserStatus.Banned))
                    Banned(this, new BannedEventArgs(null, userX.RoomItem.Reason));
            }

            Members.AsInternalImpl().Clear();
            IsInConference = false;
        });
    }

    private void room_OnJoin(object sender, IPresence presence)
    {
        if (presence.From is {} from)
            ConferenceId = from;

        dispatcher.InvokeAsyncIfRequired(() =>
        {
            var senderRoom = (IRoom)sender;
            if (waitingForPassword)
                waitingForPassword = false;

            Joined(this, new ConferenceJoinEventArgs());
            foreach (var participant in senderRoom.Participants)
            {
                if (!Members.Any(i => i.ConferenceUserId == participant.RoomParticipantJid))
                {
                    var member = new ConferenceMember(logger, session, this, participant, senderRoom)
                    {
                        AvatarUrl = AvatarsManager.GetFromCache(string.Empty)
                    };
                    Members.AsInternalImpl().Add(member);
                    ProcessAvatar(participant.Presence, member);
                }
            }

            IsInConference = true;
        });
    }

    private void room_OnParticipantLeave(object sender, IMucParticipant participant)
    {
        dispatcher.InvokeAsyncIfRequired(() =>
        {
            var senderRoom = (IRoom)sender;

            //HANDLE NICK CHANGE
            bool nickChangeAction = false;
            string newNick = string.Empty;

            var memberObj = Members.FirstOrDefault(i => i.Nick == participant.Nick);
            if (memberObj == null)
                return;
            var userX = dataExtractor.GetExtendedUserData(participant.Presence);
            var nickChange = userX?.Status.Contains(MucUserStatus.NewRoomNickname) == true ? userX : null;
            var adminItem = nickChange == null ? null : dataExtractor.GetAdminItem(nickChange);
            if (!string.IsNullOrEmpty(adminItem?.Nick))
            {
                newNick = adminItem!.Nick;
                string oldNick = participant.Nick;
                NickChange(this, new NickChangeEventArgs(oldNick, newNick));
                nickChangeAction = true;
            }

            if (nickChangeAction && participant.RoomParticipantJid == ConferenceId)
            {
                var member = memberObj;

                var newParticipant = senderRoom.Participants.FirstOrDefault(i => i.Nick == newNick);
                if (newParticipant == null)
                {
                    //LOG?
                }
                else
                {

                    Members.AsInternalImpl().Add(
                        new ConferenceMember(logger, session, this, newParticipant, senderRoom)
                        {
                            AvatarUrl = member.AvatarUrl
                        });
                }

                ConferenceId = ConferenceId.WithResource(newNick);
            }
            //HANDLE NICK CHANGE (END)

            if (!nickChangeAction)
                ParticipantLeave(this, new ConferenceMemberEventArgs(memberObj));

            Members.AsInternalImpl().Remove(i => participant.RoomParticipantJid.Equals(i.ConferenceUserId));
        });
    }

    private void room_OnParticipantJoin(object sender, IMucParticipant participant)
    {
        dispatcher.InvokeAsyncIfRequired(() =>
        {
            var senderRoom = (IRoom)sender;

            if (!Members.Any(i => i.ConferenceUserId == participant.RoomParticipantJid))
            {
                var member = new ConferenceMember(logger, session, this, participant, senderRoom)
                {
                    AvatarUrl = AvatarsManager.GetFromCache(string.Empty)
                };
                Members.AsInternalImpl().Add(member);
                if (IsInConference) //hack :)
                    ParticipantJoin(this, new ConferenceMemberEventArgs(member));

                ProcessAvatar(participant.Presence, member);
            }
        });
    }

    private void room_OnRoomMessage(object sender, IMessage msg)
    {
        dispatcher.InvokeAsyncIfRequired(() =>
        {
            var senderRoom = (IRoom)sender;
            if (string.IsNullOrEmpty(msg.Body)) return;

            var stamp = dataExtractor.GetDelayStamp(msg) ?? DateTime.Now;
            Messages.AsInternalImpl().Add(new ConferenceUserMessage(dataExtractor, session, senderRoom, msg)
            {
                Timestamp = stamp
            });
        });
    }

    public event EventHandler<CaptchaEventArgs> CaptchaRequirement = delegate { };

    private bool captchaMode = false;
    private string captchaChallenge = null;

    private void room_OnAdminMessage(object sender, IMessage msg)
    {
        dispatcher.InvokeAsyncIfRequired(() =>
        {
            // CAPTCHA required
            var captcha = dataExtractor.GetCaptchaRequest(msg);
            if (captcha != null)
            {
                captchaChallenge = captcha.CaptchaChallenge;
                CaptchaRequirement(
                    this,
                    new CaptchaEventArgs(ImageUtils.Base64ToBitmapImage(captcha.CaptchaImageBodyBase64)));
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
            Messages.AsInternalImpl().Add(new ConferenceUserMessage(dataExtractor, session, (IRoom)sender, msg));
        });
    }

    private void room_OnSelfMessage(object _, IMessage msg)
    {
        dispatcher.InvokeAsyncIfRequired(() =>
        {
            if (string.IsNullOrEmpty(msg.Body)) return;
            Messages.AsInternalImpl().Add(new ConferenceUserMessage(dataExtractor, session, msg, true));
        });
    }

    private void room_OnSubjectChange(object sender, IMessage msg)
    {
        dispatcher.InvokeAsyncIfRequired(() =>
        {
            if (msg.From != null && !string.IsNullOrEmpty(msg.From?.Resource))
                SubjectChanged(this, new SubjectChangedEventArgs(msg.From!.Value.Resource, msg.Subject));

            Subject = msg.Subject;
        });
    }

    private void ProcessAvatar(IPresence presence, IConferenceMember member)
    {
        var hasAvatar = ((AvatarsManager)AvatarsManager).ProcessAvatarChangeHash(presence, ConferenceId);
        if (member.IsSubscribed) return;

        if (!hasAvatar)
            AvatarsManager.SendAvatarRequest(member.ConferenceUserId).NoAwait(logger);

        member.IsSubscribed = true;
    }

    #region Implementation of ISessionHolder

    public IUserSession Session => session;

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

    private Jid conferenceId;
    public Jid ConferenceId
    {
        get
        {
            lock(_stateLock)
                return conferenceId;
        }
        private set
        {
            lock (_stateLock)
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
        room.SetSubject(subj);
    }

    public async Task SendPublicMessage(string body)
    {
        if (captchaMode)
        {
            var response = await session.SendCaptchaAnswer(ConferenceId, captchaChallenge, body);
            if (response.Error == null)
                captchaMode = false;
            else
            {
                //let's rejoin
                room.Leave("");
                room.Join();
                InvalidCaptchaCode(this, EventArgs.Empty);
            }
            return;
        }

        if (waitingForPassword)
        {
            //let's rejoin
            room.Leave("");
            room.Join(body);
            return;
        }

        room.SendPublicMessage(body);
    }

    public bool ChangeNick(string value)
    {
        if (IsInConference)
        {
            //TODO: GLOBAL VALIDATOR!!!
            if (!string.IsNullOrWhiteSpace(value) && value.Length < 30 &&
                !value.Contains("@") && !value.Contains("/"))
                room.SetNickname(value);
        }

        return true;
    }

    private bool isNickConflictMode = false;

    public void RejoinWithNewNick(string nick)
    {
        ConferenceId = ConferenceId.WithResource(nick);
        if (session.IsAuthenticated)
            Authenticated(this, new AuthenticationEventArgs());
    }

    public void ChangeNickAndStatus(string nick, StatusType statusType, string status) =>
        Session.SendPresence(new PresenceDetails
        {
            To = ConferenceId.WithResource(nick),
            StatusText = status,
            StatusType = statusType
        });

    internal void RaiseSomebodyChangedHisStatusEvent(IConferenceMember member)
    {
        SomebodyChangedHisStatus(this, new ConferenceMemberEventArgs(member));
    }

    public event EventHandler NotFound = delegate { };
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

    public override bool Equals(object obj)
    {
        if (obj == null)
            return false;
        return ConferenceId.Equals(((Conference)obj).ConferenceId);
    }

    public override int GetHashCode()
    {
        return ConferenceId.GetHashCode();
    }
}
