using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

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
