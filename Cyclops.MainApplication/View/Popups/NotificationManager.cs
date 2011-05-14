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
        public const int StaysOpen = 4000;

        public static void NotifyPrivate(PrivateMessage privateMessage, BitmapImage bitmapImage)
        {
            var session = ChatObjectFactory.GetSession();
            var fromConference = session.Conferences.FirstOrDefault(i => i.ConferenceId.BaresEqual(privateMessage.AuthorId));
            BitmapImage avatar = null;
            if (fromConference != null)
            {
                var member = fromConference.Members.FirstOrDefault(i => i.ConferenceUserId.Equals(privateMessage.AuthorId));
                if (member != null)
                    avatar = member.AvatarUrl;
            }

            var privateNotificationViewModel = new PrivateNotificationViewModel();
            privateNotificationViewModel.PrivateMessage = privateMessage;
            if (avatar != null)
                privateNotificationViewModel.Avatar = avatar;


            var notification = new PrivateNotification
                                   {
                                       StayOpenMilliseconds = StaysOpen,
                                       DataContext = privateNotificationViewModel
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
