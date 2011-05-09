﻿using System.Net.Mime;
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

        public IChatAreaView View { get; set; }

        protected ChatAreaViewModel(IChatAreaView view)
        {
            View = view;
            DecoratorsRegistry.NickClick += DecoratorsRegistryNickClick;
        }

        private void DecoratorsRegistryNickClick(object sender, StringEventArgs e)
        {
            if (!IsActive || string.IsNullOrEmpty(e.String))
                return;

            SendPublicMessageToUser(e.String + ": ");
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