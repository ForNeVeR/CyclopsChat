using System;
using System.Linq;
using System.Windows;
using System.Windows.Media.Imaging;
using Cyclops.Core;
using Cyclops.MainApplication.ViewModel;
using static Cyclops.Wpf.DesignerUtil;

namespace Cyclops.MainApplication.View.Popups
{
    public class PrivateNotificationViewModel : ViewModelBaseEx
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
                OnPropertyChanged();
            }
        }

        private BitmapImage avatar;
        public BitmapImage Avatar
        {
            get { return avatar; }
            set
            {
                avatar = value;
                OnPropertyChanged();
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
