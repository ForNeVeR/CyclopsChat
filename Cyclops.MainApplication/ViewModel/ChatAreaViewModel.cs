using GalaSoft.MvvmLight;

namespace Cyclops.MainApplication.ViewModel
{
    public abstract class ChatAreaViewModel : ViewModelBase
    {
        private int unreadMessagesCount;
        private bool isActive;

        public virtual bool IsPrivate
        {
            get { return false; }
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
    }
}