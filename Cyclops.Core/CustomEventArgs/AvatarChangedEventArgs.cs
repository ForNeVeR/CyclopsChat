using System;
using System.Windows.Media.Imaging;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.CustomEventArgs
{
    public class AvatarChangedEventArgs : EventArgs
    {
        public Jid UserId { get; private set; }
        public BitmapImage BitmapImage { get; private set; }

        public AvatarChangedEventArgs(Jid userId, BitmapImage bitmapImage)
        {
            UserId = userId;
            BitmapImage = bitmapImage;
        }
    }
}
