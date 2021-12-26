using System;
using System.Windows.Media.Imaging;
using Cyclops.Xmpp.Data;

namespace Cyclops.Core.CustomEventArgs
{
    public class AvatarChangedEventArgs : EventArgs
    {
        public IEntityIdentifier UserId { get; private set; }
        public BitmapImage BitmapImage { get; private set; }

        public AvatarChangedEventArgs(IEntityIdentifier userId, BitmapImage bitmapImage)
        {
            UserId = userId;
            BitmapImage = bitmapImage;
        }
    }
}
