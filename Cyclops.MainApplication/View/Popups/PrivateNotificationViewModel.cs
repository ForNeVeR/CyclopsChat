using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;
using Cyclops.Core;
using GalaSoft.MvvmLight;

namespace Cyclops.MainApplication.View.Popups
{
    public class PrivateNotificationViewModel : ViewModelBase
    {
        public PrivateNotificationViewModel()
        {
            if (IsInDesignMode)
            {
                PrivateMessage = new PrivateMessage
                                     {
                                         AuthorNick = "Freddy Mercury",
                                         Body = "Show must go on! show must go on! Inside my heart is breaking my make-up may be flaking but my smile still stays on",
                                     };
            }
            Avatar = new BitmapImage();
            Avatar.BeginInit();
            Avatar.UriSource = new Uri("pack://application:,,,/Cyclops.MainApplication;component/Resources/testavatar.png");
            Avatar.EndInit();

        }

        private PrivateMessage privateMessage;
        public PrivateMessage PrivateMessage
        {
            get { return privateMessage; }
            set
            {
                privateMessage = value;
                RaisePropertyChanged("PrivateMessage");
            }
        }

        private BitmapImage avatar;
        public BitmapImage Avatar
        {
            get { return avatar; }
            set
            {
                avatar = value;
                RaisePropertyChanged("Avatar");
            }
        }

        public void OnRequestActivate()
        {
            var main = ApplicationContext.Current.MainViewModel;
            main.SelectedPrivate = main.PrivateViewModels.FirstOrDefault(i => i.Participant != null && i.Participant.Equals(PrivateMessage.AuthorId));

            Window window = Application.Current.MainWindow;
            window.Show();
            window.WindowState = WindowState.Normal;
            //workaround:)
            window.Activate();
            window.Topmost = true;  // important
            window.Topmost = false; // important
            window.Focus();   
        }
    }
}
