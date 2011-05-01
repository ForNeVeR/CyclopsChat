using System.Net.Mime;
using Cyclops.MainApplication.MessageDecoration;
using Cyclops.MainApplication.MessageDecoration.Decorators;
using GalaSoft.MvvmLight;

namespace Cyclops.MainApplication.ViewModel
{
    public abstract class ChatAreaViewModel : ViewModelBase
    {
        private int unreadMessagesCount;
        private string currentlyTypedMessage;
        private bool isActive;

        public virtual bool IsPrivate
        {
            get { return false; }
        }

        protected ChatAreaViewModel()
        {
            DecoratorsRegistry.NickClick += DecoratorsRegistryNickClick;
        }

        private void DecoratorsRegistryNickClick(object sender, StringEventArgs e)
        {
            if (!IsActive || string.IsNullOrEmpty(e.String))
                return;

            CurrentlyTypedMessage = e.String + ": " + CurrentlyTypedMessage;
        }

        public bool IsActive
        {
            get { return isActive; }
            set
            {
                isActive = value;
                RaisePropertyChanged("IsActive");

                if (value)
                    UnreadMessagesCount = 0;
            }
        }

        public int UnreadMessagesCount
        {
            get { return unreadMessagesCount; }
            set
            {
                unreadMessagesCount = value;
                RaisePropertyChanged("UnreadMessagesCount");
            }
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
    }
}