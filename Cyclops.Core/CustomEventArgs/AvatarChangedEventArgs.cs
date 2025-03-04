using System;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.CustomEventArgs;

public class AvatarChangedEventArgs : EventArgs
{
    public Jid UserId { get; private set; }
    public byte[] BitmapImage { get; private set; }

    public AvatarChangedEventArgs(Jid userId, byte[] bitmapImage)
    {
        UserId = userId;
        BitmapImage = bitmapImage;
    }
}
