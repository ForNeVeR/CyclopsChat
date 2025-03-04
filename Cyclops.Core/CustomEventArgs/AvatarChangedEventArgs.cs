using System;
using System.Drawing;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.CustomEventArgs;

public class AvatarChangedEventArgs : EventArgs
{
    public Jid UserId { get; private set; }
    public Image BitmapImage { get; private set; }

    public AvatarChangedEventArgs(Jid userId, Image bitmapImage)
    {
        UserId = userId;
        BitmapImage = bitmapImage;
    }
}
