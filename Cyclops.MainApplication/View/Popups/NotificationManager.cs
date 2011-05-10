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
        
        public static void NotifyPrivate(PrivateMessage privateMessage, BitmapImage bitmapImage)
        {
            var privateNotification = new PrivateNotification
                                          {
                                              StayOpenMilliseconds = 10000,
                                              DataContext = new PrivateNotificationViewModel
                                                                {
                                                                    //Avatar = bitmapImage,
                                                                    PrivateMessage = privateMessage
                                                                }
                                          };
            privateNotification.Show();
            privateNotification.Notify();
        }
    }
}
