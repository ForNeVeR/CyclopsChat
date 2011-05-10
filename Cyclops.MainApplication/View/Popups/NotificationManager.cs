using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Cyclops.Core;

namespace Cyclops.MainApplication.View.Popups
{
    public static class NotificationManager
    {
        public const int StaysOpen = 10000;

        public static void NotifyPrivate(PrivateMessage privateMessage, BitmapImage bitmapImage)
        {
            var notification = new PrivateNotification
                                   {
                                       StayOpenMilliseconds = StaysOpen,
                                       DataContext = new PrivateNotificationViewModel
                                                         {
                                                             //Avatar = bitmapImage,
                                                             PrivateMessage = privateMessage
                                                         }
                                   };
            notification.Show();
            notification.Notify();
        }

        public static void NotifyError(string title, string body)
        {
            var notification = new ErrorNotification
                                   {
                                       StayOpenMilliseconds = StaysOpen,
                                       DataContext = new ErrorNotificationViewModel
                                                         {
                                                             Body = body,
                                                             Title = title
                                                         },
                                   };
            notification.Show();
            notification.Notify();
        }
    }
}
