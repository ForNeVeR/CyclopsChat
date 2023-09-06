using System.Collections.ObjectModel;
using Cyclops.Core;
using Cyclops.MainApplication.Controls;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.MainApplication.ViewModel
{
    public class PrivateViewModel : ChatAreaViewModel
    {
        private readonly string currentlyTypedMessage;
        private ObservableCollection<MessageViewModel> messages;

        public PrivateViewModel(IChatAreaView view) : base(view)
        {
            Messages = new ObservableCollection<MessageViewModel>();
            Messages.CollectionChanged += MessagesCollectionChanged;
        }

        void MessagesCollectionChanged(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            if (!IsActive)
            {
                UnreadMessagesCount++;
            }

            ApplicationSounds.PlayOnIcomingPrivate(this);
        }


        protected override void CloseAction()
        {
            ApplicationContext.Current.MainViewModel.PrivateViewModels.Remove(this);
        }

        public Jid Participant { get; set; }

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

        protected override void OnSendMessage()
        {
            if (string.IsNullOrEmpty(CurrentlyTypedMessage))
                return;


            ChatObjectFactory.GetSession().SendPrivate(Participant, CurrentlyTypedMessage);

            Messages.Add(new MessageViewModel(new PrivateMessage
                                                  {
                                                      AuthorNick = Localization.Conference.Me,
                                                      IsSelfMessage = true,
                                                      Body = RemoveEndNewLineSymbol(CurrentlyTypedMessage)
                                                  }));
            CurrentlyTypedMessage = string.Empty;
        }

        public override bool IsPrivate => true;

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Participant.Resource))
                return Participant.Local;
            return string.Format("{0} ({1})", Participant.Resource, Participant.Local);
        }
    }
}
