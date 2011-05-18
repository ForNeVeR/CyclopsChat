using System.Net.Mime;
using Cyclops.Core;
using Cyclops.MainApplication.MessageDecoration;
using Cyclops.MainApplication.MessageDecoration.Decorators;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    public abstract class ChatAreaViewModel : ViewModelBaseEx
    {
        private int unreadMessagesCount;
        private string currentlyTypedMessage;
        private bool isActive;

        public virtual bool IsPrivate
        {
            get { return false; }
        }

        public IChatAreaView View { get; set; }
        public RelayCommand ClearOutputArea { get; private set; }

        protected ChatAreaViewModel(IChatAreaView view)
        {
            View = view;
            ClearOutputArea = new RelayCommand(() => View.ClearOutputArea());
            DecoratorsRegistry.NickClick += DecoratorsRegistryNickClick;
        }

        private void DecoratorsRegistryNickClick(object sender, NickEventArgs e)
        {
            if (!IsActive || string.IsNullOrEmpty(e.Nick))
                return;

            SendPublicMessageToUser(e.Nick + ": ");
            if (e.Id != null)
                OnNickClick(e.Id);
        }

        protected virtual void OnNickClick(IEntityIdentifier id)
        {
        }

        protected void SendPublicMessageToUser(string nick)
        {
            if (CurrentlyTypedMessage == null)
                CurrentlyTypedMessage = string.Empty;

            if (View.InputBoxSelectionLength == 0)
                CurrentlyTypedMessage = CurrentlyTypedMessage.Insert(View.InputBoxSelectionStart, nick);
            else
                CurrentlyTypedMessage = CurrentlyTypedMessage.Remove(0, View.InputBoxSelectionLength).Insert(View.InputBoxSelectionStart, nick);
            View.InputBoxSelectionLength = 0;
            View.InputBoxSelectionStart = CurrentlyTypedMessage.Length;
            View.InputboxFocus();
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