using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Cyclops.Core;
using Cyclops.Core.CustomEventArgs;
using Cyclops.MainApplication.Controls;
using Cyclops.MainApplication.View.Dialogs;
using Cyclops.Xmpp.Data;

namespace Cyclops.MainApplication.ViewModel
{
    public partial class ConferenceViewModel : ChatAreaViewModel
    {
        private IConference conference;
        private ObservableCollection<MessageViewModel> messages;
        private string newNick;
        private IConferenceMember selectedMember;
        private string statusText;
        private IChatObjectsValidator validator;

        public ConferenceViewModel(IChatAreaView view, IConference conference) : base(view)
        {
            validator = ChatObjectFactory.GetValidator();
            Conference = conference;

            SubscribeToEvents();

            Messages = new ObservableCollection<MessageViewModel>();
            Conference.Messages.SynchronizeWith(Messages, OnInsertMessage);

            InitializeCommands();
            newNick = conference.ConferenceId.Resource;

            AddNotifyMessage(Localization.Conference.Entering);
        }

        protected override void OnNickClick(IEntityIdentifier id)
        {
            SelectedMember = Conference.Members.FirstOrDefault(i => i.ConferenceUserId.Equals(id));
        }

        private void SubscribeToEvents()
        {
            Conference.Joined += ConferenceJoined;
            Conference.Banned += ConferenceBanned;
            Conference.NickChange += ConferenceNickChange;
            Conference.Kicked += ConferenceKicked;
            Conference.RoleChanged += RoleChanged;
            Conference.AccessDenied += ConferenceAccessDenied;
            Conference.InvalidCaptchaCode += ConferenceInvalidCaptchaCode;
            Conference.Disconnected += ConferenceDisconnected;
            Conference.BeginJoin += ConferenceBeginJoin;
            Conference.ServiceUnavailable += ConferenceServiceUnavailable;
            Conference.NotFound += ConferenceNotFound;
            Conference.StartReconnectTimer += ConferenceStartReconnectTimer;
            Conference.CantChangeSubject += ConferenceCantChangeSubject;
            Conference.SubjectChanged += ConferenceSubjectChanged;
            Conference.CaptchaRequirment += ConferenceCaptchaRequirment;
            Conference.MethodNotAllowedError += ConferenceMethodNotAllowedError;
            Conference.ParticipantJoin += ConferenceParticipantJoin;
            Conference.SomebodyChangedHisStatus += ConferenceSomebodyChangedHisStatus;
            Conference.ParticipantLeave += ConferenceParticipantLeave;
            Conference.Messages.CollectionChanged += ConferenceMessagesCollectionChanged;
        }

        private void ConferenceNotFound(object sender, EventArgs e)
        {
            SoundOnSystemMessage();
            AddSystemMessage(Localization.Conference.NotFoundError);
        }

        private void RoleChanged(object sender, RoleChangedEventArgs e)
        {
            if (Settings.ShowRoleChanges)
                AddNotifyMessage(Localization.Conference.ChangeRole, e.To, LocalizeRole(e.Role));
        }

        private string LocalizeRole(Role role)
        {
            switch (role)
            {
                case Role.Banned:
                    return Localization.Conference.RoleBanned;
                case Role.Kicked:
                    return Localization.Conference.RoleKicked;
                case Role.Devoiced:
                    return Localization.Conference.RoleDevoiced;
                case Role.Regular:
                    return Localization.Conference.RoleRegular;
                case Role.Member:
                    return Localization.Conference.RoleMember;
                case Role.Moder:
                    return Localization.Conference.RoleModer;
                case Role.Admin:
                    return Localization.Conference.RoleAdmin;
                case Role.Owner:
                    return Localization.Conference.RoleOwner;
                default:
                    throw new ArgumentOutOfRangeException("role");
            }
        }

        private void ConferenceMessagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems.OfType<IConferenceMessage>().Any(i => !i.IsFromHistory))
                ApplicationSounds.PlayOnIcomingPublic(this);
        }

        private void ConferenceSomebodyChangedHisStatus(object sender, ConferenceMemberEventArgs e)
        {
            if (Settings.ShowStatusChangingMessages)
            {
                ApplicationSounds.PlayOnStatusChanging(this);
                AddNotifyMessage(Localization.Conference.StatusChangingFormat, 
                    e.Member.Nick, e.Member.StatusType, e.Member.StatusText);
            }
        }

        private void ConferenceServiceUnavailable(object sender, EventArgs e)
        {
            SoundOnSystemMessage();
            AddSystemMessage(Localization.Conference.ServiceUnavailableError);
        }

        private void ConferenceParticipantLeave(object sender, ConferenceMemberEventArgs e)
        {
            ApplicationSounds.PlayOnUserLeave(this);
            if (ApplicationContext.Current.ApplicationSettings.ShowEntryAndExits)
                AddNotifyMessage(Localization.Conference.UserLeaveMessage, e.Member.Nick);
        }

        private void ConferenceParticipantJoin(object sender, ConferenceMemberEventArgs e)
        {
            ApplicationSounds.PlayOnUserJoin(this);
            if (ApplicationContext.Current.ApplicationSettings.ShowEntryAndExits)
                AddNotifyMessage(Localization.Conference.UserJoinMessage, e.Member.Nick);
        }

        private void SoundOnSystemMessage()
        {
            ApplicationSounds.PlayOnSystemMessage(this);
        }

        private void ConferenceMethodNotAllowedError(object sender, EventArgs e)
        {
            SoundOnSystemMessage();
            AddSystemMessage(Localization.Conference.MethodNotAllowedError);
        }

        private void ConferenceSubjectChanged(object sender, SubjectChangedEventArgs e)
        {
            SoundOnSystemMessage();
            AddNotifyMessage(Localization.Conference.ChangeSubjectByParticipant, e.Author, e.NewSubject);
        }

        private void ConferenceCantChangeSubject(object sender, EventArgs e)
        {
            SoundOnSystemMessage();
            AddSystemMessage(Localization.Conference.ChangeSubjectError);
        }

        public IConferenceMember SelectedMember
        {
            get { return selectedMember; }
            set
            {
                selectedMember = value;
                RaisePropertyChanged("SelectedMember");
            }
        }

        public override bool IsConference
        {
            get { return true; }
        }

        public IConference Conference
        {
            get { return conference; }
            set
            {
                conference = value;
                RaisePropertyChanged("Conference");
            }
        }

        public ObservableCollection<MessageViewModel> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
                RaisePropertyChanged("Messages");
            }
        }

        public string NewNick
        {
            get { return newNick; }
            set
            {
                if (Conference.IsInConference && validator.ValidateName(value))
                {
                    newNick = value;
                    Conference.ChangeNick(value);
                }
                RaisePropertyChanged("NewNick");
            }
        }

        private void ConferenceStartReconnectTimer(object sender, EventArgs e)
        {
            AddNotifyMessage(Localization.Conference.ReconnectStart, 10 /*config context!*/);
        }

        private void ConferenceBeginJoin(object sender, EventArgs e)
        {
            AddNotifyMessage(Localization.Conference.Entering);
        }

        private void ConferenceNickChange(object sender, NickChangeEventArgs e)
        {
            SoundOnSystemMessage();
            AddNotifyMessage(Localization.Conference.IsNowKnownAs, e.OldNick, e.NewNick);
        }

        private void ConferenceAccessDenied(object sender, EventArgs e)
        {
            SoundOnSystemMessage();
            AddSystemMessage(Localization.Conference.OnlyMembersAccess);
        }

        private MessageViewModel OnInsertMessage(IConferenceMessage msg)
        {
            if (!IsActive) UnreadMessagesCount++;
            return new MessageViewModel(msg);
        }

        private void ConferenceInvalidCaptchaCode(object sender, EventArgs e)
        {
            SoundOnSystemMessage();
            AddSystemMessage(Localization.Conference.InvalidCaptchaCode);
        }

        private void ConferenceCaptchaRequirment(object sender, CaptchaEventArgs e)
        {
            SoundOnSystemMessage();
            Messages.Add(new MessageViewModel(
                             new CaptchaSystemMessage
                                 {
                                     Body = string.Format(Localization.Conference.CaptchaMessage + Environment.NewLine),
                                     Bitmap = e.BitmapImage
                                 }));
        }

        private void ConferenceJoined(object sender, ConferenceJoinEventArgs e)
        {
            string text = "";
            if (e.ErrorKind == ConferenceJoinErrorKind.NickConflict)
            {
                text = string.Format(
                    Localization.Conference.NickConflictMessage,
                    Conference.ConferenceId.Resource);
                AddSystemMessage(text);
                ChangeConflictedNick();
                return;
            }
            if (e.ErrorKind == ConferenceJoinErrorKind.Banned)
                text = Localization.Conference.AlreadyBannedMessage;
            else if (e.ErrorKind == ConferenceJoinErrorKind.PasswordRequired)
                text = Localization.Conference.RoomHasPassword;
            else if (e.ErrorKind == ConferenceJoinErrorKind.WrongPassword)
                text = Localization.Conference.InvalidPasswordErrorMessage;

            if (!string.IsNullOrEmpty(text))
                AddSystemMessage(text);
            else
                AddNotifyMessage(Localization.Conference.EnteredToTheRoom);
        }

        private void ChangeConflictedNick()
        {
            InputDialog.ShowForEdit(string.Format(Localization.Conference.NewNick, Conference.ConferenceId.User), 
                Conference.ConferenceId.Resource, TryRejoinWithNewNick, NewNickValidator);
        }

        private void TryRejoinWithNewNick(string nick)
        {
            Conference.RejoinWithNewNick(nick);
        }

        private bool NewNickValidator(string nick)
        {
            return !string.IsNullOrWhiteSpace(nick) && Conference.ConferenceId.Resource != nick && nick.Length < 30;
        }

        private void ConferenceDisconnected(object sender, DisconnectEventArgs e)
        {
            SoundOnSystemMessage();
            AddSystemMessage(Localization.Conference.DisconnectMessage, e.ErrorMessage);
        }

        private void ConferenceKicked(object sender, KickedEventArgs e)
        {
            SoundOnSystemMessage();
            AddSystemMessage(Localization.Conference.KickMessage, e.Reason);
        }

        private void ConferenceBanned(object sender, BannedEventArgs e)
        {
            SoundOnSystemMessage();
            AddSystemMessage(Localization.Conference.BanMessage, e.Reason);
        }

        private void AddSystemMessage(string messageFormat, params object[] args)
        {
            Messages.Add(new MessageViewModel(new SystemConferenceMessage
                                                  {Body = string.Format(messageFormat, args), IsErrorMessage = true}));
        }

        private void AddNotifyMessage(string messageFormat, params object[] args)
        {
            Messages.Add(new MessageViewModel(new SystemConferenceMessage {Body = string.Format(messageFormat, args)}));
        }

        protected override void OnSendMessage()
        {
            if (string.IsNullOrEmpty(CurrentlyTypedMessage))
                return;
            Conference.SendPublicMessage(RemoveEndNewLineSymbol(CurrentlyTypedMessage));
            CurrentlyTypedMessage = string.Empty;
        }

        protected override void CloseAction()
        {
            Conference.LeaveAndClose();
        }

        public override string ToString()
        {
            return Conference.ConferenceId.User;
        }
    }
}