using System;
using System.Collections.ObjectModel;
using System.Linq;
using Cyclops.Core;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Resource;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    public class ConferenceViewModel : ViewModelBase
    {
        private IConference conference;
        private string currentlyTypedMessage;
        private ObservableCollection<MessageViewModel> messages;
        private IConferenceMember selectedMember;

        public ConferenceViewModel(IConference conference)
        {
            Conference = conference;
            Conference.Joined += ConferenceJoined;
            Conference.Banned += ConferenceBanned;
            Conference.Kicked += ConferenceKicked;
            Conference.InvalidCaptchaCode += ConferenceInvalidCaptchaCode;
            Conference.Disconnected += ConferenceDisconnected;

            Conference.CaptchaRequirment += ConferenceCaptchaRequirment;

            Conference.Members.CollectionChanged += MembersCollectionChanged;
            Messages = new ObservableCollection<MessageViewModel>();
            Conference.Messages.SynchronizeWith(Messages, msg => new MessageViewModel(msg));

            SendMessage = new RelayCommand(OnSendMessage, () => !string.IsNullOrEmpty(CurrentlyTypedMessage));
            StartPrivateWithSelectedMember = new RelayCommand(StartPrivateWithSelectedMemberAction, () => SelectedMember != null && SelectedMember.ConferenceUserId != null);
        }

        private void ConferenceInvalidCaptchaCode(object sender, EventArgs e)
        {
            AddSystemMessage("Invalid code.");
        }

        private void ConferenceCaptchaRequirment(object sender, CaptchaEventArgs e)
        {
            Messages.Add(new MessageViewModel(
                new CaptchaSystemMessage
                    {
                        Body = string.Format("Proof you are not the bot:\n"), 
                        Bitmap = e.BitmapImage
                    }));
        }

        private void MembersCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Add)
                foreach (var item in e.NewItems.OfType<IConferenceMember>())
                    AddNotifyMessage("{0} has joined to us", item.Nick);
            else if (e.Action == System.Collections.Specialized.NotifyCollectionChangedAction.Remove)
                foreach (var item in e.OldItems.OfType<IConferenceMember>())
                    AddNotifyMessage("{0} has left the room", item.Nick);
        }

        private void StartPrivateWithSelectedMemberAction()
        {
            if (SelectedMember != null)
                ChatObjectFactory.GetSession().StartPrivate(SelectedMember.ConferenceUserId);
        }

        public string CurrentlyTypedMessage
        {
            get { return currentlyTypedMessage; }
            set
            {
                currentlyTypedMessage = value;
                RaisePropertyChanged("CurrentlyTypedMessage");
            }
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

        private void ConferenceJoined(object sender, ConferenceJoinEventArgs e)
        {
            string text = "";
            if (e.ErrorKind == ConferenceJoinErrorKind.NickConflict)
                text = string.Format(
                    "Nick conflict ({0}). Please, change it and rejoin.",
                    Conference.ConferenceId.Resource);
            else if (e.ErrorKind == ConferenceJoinErrorKind.Banned)
                text = "You are banned in this room.";
            else if (e.ErrorKind == ConferenceJoinErrorKind.PasswordRequired)
                text = "This room has a password :( (not implemented yet).";

            if (!string.IsNullOrEmpty(text))
                AddSystemMessage(text);
            else
                AddNotifyMessage("Entered");
        }

        private void ConferenceDisconnected(object sender, DisconnectEventArgs e)
        {
            AddSystemMessage("Disconnect due '{0}'.", e.ErrorMessage);
        }

        private void ConferenceKicked(object sender, KickedEventArgs e)
        {
            AddSystemMessage("You are kicked due the reason: '{0}'.", e.Reason);
        }

        private void ConferenceBanned(object sender, BannedEventArgs e)
        {
            AddSystemMessage("You are banned due the reason: '{0}'.", e.Reason);
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