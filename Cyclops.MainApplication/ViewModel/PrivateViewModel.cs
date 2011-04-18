using System.Collections.ObjectModel;
using Cyclops.Core;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    public class PrivateViewModel : ViewModelBase
    {
        private string currentlyTypedMessage;
        private ObservableCollection<MessageViewModel> messages;

        public PrivateViewModel()
        {
            Messages = new ObservableCollection<MessageViewModel>();
            SendMessage = new RelayCommand(OnSendMessage, () => !string.IsNullOrEmpty(CurrentlyTypedMessage));
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

        public IEntityIdentifier Participant { get; set; }

        public RelayCommand SendMessage { get; private set; }

        public IConference Conference { get; set; }

        public ObservableCollection<MessageViewModel> Messages
        {
            get { return messages; }
            set
            {
                messages = value;
                RaisePropertyChanged("Messages");
            }
        }

        private void OnSendMessage()
        {
            if (string.IsNullOrEmpty(CurrentlyTypedMessage))
                return;


            ChatObjectFactory.GetSession().SendPrivate(Participant, CurrentlyTypedMessage);

            Messages.Add(new MessageViewModel(new PrivateMessage {AuthorNick = "Me", IsSelfMessage = true, Body = CurrentlyTypedMessage}));
            CurrentlyTypedMessage = string.Empty;
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", Participant.Resource, Participant.User);
        }
    }
}