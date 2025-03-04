using System;

namespace Cyclops.Core.CustomEventArgs;

public class CaptchaEventArgs : EventArgs
{
    public byte[] BitmapImage { get; private set; }

    public CaptchaEventArgs(byte[] bitmapImage)
    {
        this.BitmapImage = bitmapImage;
    }
}
