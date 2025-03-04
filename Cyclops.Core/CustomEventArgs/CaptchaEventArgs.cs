using System;
using System.Drawing;

namespace Cyclops.Core.CustomEventArgs;

public class CaptchaEventArgs : EventArgs
{
    public Image BitmapImage { get; private set; }

    public CaptchaEventArgs(Image bitmapImage)
    {
        this.BitmapImage = bitmapImage;
    }
}
