using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Cyclops.Core;
using Cyclops.Core.CustomEventArgs;
using Cyclops.MainApplication.MessageDecoration;
using Cyclops.MainApplication.MessageDecoration.Decorators;
using Cyclops.MainApplication.View.Dialogs;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    public class ConferenceViewModel : ChatAreaViewModel
    {
        private IConference conference;
        private ObservableCollection<MessageViewModel> messages;
        private string newNick;
        private IConferenceMember selectedMember;
        private string statusText;
        private IChatObjectsValidator validator;

        public ConferenceViewModel(IConference conference)
        {
            validator = ChatObjectFactory.GetValidator();

            Conference = conference;
            Conference.Joined += ConferenceJoined;
            Conference.Banned += ConferenceBanned;
            Conference.NickChange += ConferenceNickChange;
            Conference.Kicked += ConferenceKicked;
            Conference.AccessDenied += ConferenceAccessDenied;
            Conference.InvalidCaptchaCode += ConferenceInvalidCaptchaCode;
            Conference.Disconnected += ConferenceDisconnected;
            Conference.BeginJoin += ConferenceBeginJoin;
            Conference.StartReconnectTimer += ConferenceStartReconnectTimer;
            Conference.CantChangeSubject += ConferenceCantChangeSubject;
            Conference.SubjectChanged += ConferenceSubjectChanged;
            Conference.CaptchaRequirment += ConferenceCaptchaRequirment;
            Conference.MethodNotAllowedError += ConferenceMethodNotAllowedError;
            Conference.ParticipantJoin += ConferenceParticipantJoin;
            Conference.ParticipantLeave += ConferenceParticipantLeave;

            Messages = new ObservableCollection<MessageViewModel>();
            Conference.Messages.SynchronizeWith(Messages, OnInsertMessage);

            SendMessage = new RelayCommand(OnSendMessage, () => !string.IsNullOrEmpty(CurrentlyTypedMessage));
            StartPrivateWithSelectedMember = new RelayCommand(StartPrivateWithSelectedMemberAction, () =>
                                                              SelectedMember != null &&
                                                              SelectedMember.ConferenceUserId != null);
            ChangeSubject = new RelayCommand(ChangeSubjectAction, () => Conference.IsInConference);
            newNick = conference.ConferenceId.Resource;
        }

        private void ConferenceParticipantLeave(object sender, ConferenceMemberEventArgs e)
        {
            AddNotifyMessage(Localization.Conference.UserLeaveMessage, e.Member.Nick);
        }

        private void ConferenceParticipantJoin(object sender, ConferenceMemberEventArgs e)
        {
            AddNotifyMessage(Localization.Conference.UserLeaveMessage, e.Member.Nick);
        }

        private void ConferenceMethodNotAllowedError(object sender, EventArgs e)
        {
            AddSystemMessage(Localization.Conference.MethodNotAllowedError);
        }

        private void ConferenceSubjectChanged(object sender, SubjectChangedEventArgs e)
        {
            AddNotifyMessage(Localization.Conference.ChangeSubjectByParticipant, e.Author, e.NewSubject);
        }

        private void ConferenceCantChangeSubject(object sender, EventArgs e)
        {
            AddSystemMessage(Localization.Conference.ChangeSubjectError);
        }

        private void ChangeSubjectAction()
        {
            DialogManager.ShowStringInputDialog(Localization.Conference.ChangeSubject, Conference.Subject, subj => Conference.ChangeSubject(subj), subj => subj.Length < 1000);
        }

        public RelayCommand SendMessage { get; private set; }
        public RelayCommand ChangeSubject { get; private set; }
        public RelayCommand StartPrivateWithSelectedMember { get; private set; }

        public IConferenceMember SelectedMember
        {
            get { return selectedMember; }
            set
            {
                selectedMember = value;
                RaisePropertyChanged("SelectedMember");
            }
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
            AddNotifyMessage(Localization.Conference.IsNowKnownAs, e.OldNick, e.NewNick);
        }

        private void ConferenceAccessDenied(object sender, EventArgs e)
        {
            AddSystemMessage(Localization.Conference.OnlyMembersAccess);
        }

        private MessageViewModel OnInsertMessage(IConferenceMessage msg)
        {
            if (!IsActive) UnreadMessagesCount++;
            return new MessageViewModel(msg);
        }

        private void ConferenceInvalidCaptchaCode(object sender, EventArgs e)
        {
            AddSystemMessage(Localization.Conference.InvalidCaptchaCode);
        }

        private void ConferenceCaptchaRequirment(object sender, CaptchaEventArgs e)
        {
            Messages.Add(new MessageViewModel(
                             new CaptchaSystemMessage
                                 {
                                     Body = string.Format(Localization.Conference.CaptchaMessage + Environment.NewLine),
                                     Bitmap = e.BitmapImage
                                 }));
        }

        private void StartPrivateWithSelectedMemberAction()
        {
            if (SelectedMember != null)
                ChatObjectFactory.GetSession().StartPrivate(SelectedMember.ConferenceUserId);
        }

        private void ConferenceJoined(object sender, ConferenceJoinEventArgs e)
        {
            string text = "";
            if (e.ErrorKind == ConferenceJoinErrorKind.NickConflict)
                text = string.Format(
                    Localization.Conference.NickConflictMessage,
                    Conference.ConferenceId.Resource);
            else if (e.ErrorKind == ConferenceJoinErrorKind.Banned)
                text = Localization.Conference.AlreadyBannedMessage;
            else if (e.ErrorKind == ConferenceJoinErrorKind.PasswordRequired)
                text = Localization.Conference.RoomHasPassword;

            if (!string.IsNullOrEmpty(text))
                AddSystemMessage(text);
            else
                AddNotifyMessage(Localization.Conference.EnteredToTheRoom);
        }

        private void ConferenceDisconnected(object sender, DisconnectEventArgs e)
        {
            AddSystemMessage(Localization.Conference.DisconnectMessage, e.ErrorMessage);
        }

        private void ConferenceKicked(object sender, KickedEventArgs e)
        {
            AddSystemMessage(Localization.Conference.KickMessage, e.Reason);
        }

        private void ConferenceBanned(object sender, BannedEventArgs e)
        {
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

        private void OnSendMessage()
        {
            if (string.IsNullOrEmpty(CurrentlyTypedMessage))
                return;
            Conference.SendPublicMessage(CurrentlyTypedMessage);
            CurrentlyTypedMessage = string.Empty;
        }

        public override string ToString()
        {
            return Conference.ConferenceId.User;
        }
    }
}