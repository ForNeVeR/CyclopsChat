using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media.Imaging;
using Cyclops.Core.CustomEventArgs;

namespace Cyclops.Core.Avatars
{
    public interface IAvatarsManager : ISessionHolder
    {
        BitmapImage GetFromCache(IEntityIdentifier id);
        void SendAvatarRequest(IEntityIdentifier id);
        event EventHandler<AvatarChangedEventArgs> AvatarChange;
    }
}
