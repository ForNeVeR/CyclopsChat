using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;

namespace Cyclops.Core.CustomEventArgs
{
    public class CaptchaEventArgs : EventArgs
    {
        public BitmapImage BitmapImage { get; private set; }

        public CaptchaEventArgs(BitmapImage bitmapImage)
        {
            this.BitmapImage = bitmapImage;
        }
    }
}
