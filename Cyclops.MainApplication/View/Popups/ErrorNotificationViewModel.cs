using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Cyclops.Core;
using Cyclops.MainApplication.ViewModel;
using GalaSoft.MvvmLight;

namespace Cyclops.MainApplication.View.Popups
{
    public class ErrorNotificationViewModel : ViewModelBaseEx
    {
        public ErrorNotificationViewModel()
        {
            if (IsInDesignMode)
            {
                Title = "Common error";
                Body = "You are not allowed to perform an action at this conference.";
            }
        }

        private string title;
        public string Title
        {
            get { return title; }
            set
            {
                title = value;
                RaisePropertyChanged("Title");
            }
        }

        private string body;
        public string Body
        {
            get { return body; }
            set
            {
                body = value;
                RaisePropertyChanged("Body");
            }
        }
    }
}
