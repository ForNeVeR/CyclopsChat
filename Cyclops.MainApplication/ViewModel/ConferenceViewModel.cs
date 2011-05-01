using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using Cyclops.Core;
using Cyclops.Core.CustomEventArgs;
using Cyclops.MainApplication.MessageDecoration;
using Cyclops.MainApplication.MessageDecoration.Decorators;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    public class ConferenceViewModel : ChatAreaViewModel
    {
        private IConference conference;
        private ObservableCollection<MessageViewModel> messages;
        private string newNick;
        private IConferenceMember selectedMember;
        private CommonInputViewModel subjectChangeViewModel;
        private string statusText;

        public ConferenceViewModel(IConference conference)
        {

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

            Conference.CaptchaRequirment += ConferenceCaptchaRequirment;

            Conference.Members.CollectionChanged += MembersCollectionChanged;
            Messages = new ObservableCollection<MessageViewModel>();
            Conference.Messages.SynchronizeWith(Messages, OnInsertMessage);

            SendMessage = new RelayCommand(OnSendMessage, () => !string.IsNullOrEmpty(CurrentlyTypedMessage));
            StartPrivateWithSelectedMember = new RelayCommand(StartPrivateWithSelectedMemberAction, () =>
                                                              SelectedMember != null &&
                                                              SelectedMember.ConferenceUserId != null);

            SubjectChangeViewModel = new CommonInputViewModel(Conference.IsInConference ? Conference.Subject : string.Empty, 
                ChangeTopicAction, ChangeTopicCanExecute);
        }

        private bool ChangeTopicCanExecute(string arg)
        {
            return arg.Length < 500; //TODO: complete validator
        }

        private void ChangeTopicAction(string obj)
        {
            Conference.ChangeSubject(obj);
        }

        public RelayCommand SendMessage { get; private set; }
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


        public CommonInputViewModel SubjectChangeViewModel
        {
            get { return subjectChangeViewModel; }
            set
            {
                subjectChangeViewModel = value;
                RaisePropertyChanged("SubjectChangeViewModel");
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

        public string StatusText
        {
            get { return statusText; }
            set
            {
                statusText = value;
                ChatObjectFactory.GetSession().ChangeStatus(StatusType.Online, value);
            }
        }

        public string NewNick
        {
            get { return newNick; }
            set
            {
                newNick = value;
                Conference.ChangeNick(value);
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

        private void MembersCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                foreach (IConferenceMember item in e.NewItems.OfType<IConferenceMember>())
                    AddNotifyMessage(Localization.Conference.UserJoinMessage, item.Nick);
            else if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (IConferenceMember item in e.OldItems.OfType<IConferenceMember>())
                    AddNotifyMessage(Localization.Conference.UserLeaveMessage, item.Nick);
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