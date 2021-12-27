using System;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.Avatars
{
    public interface IAvatarsManager : ISessionHolder
    {
        bool DoesCacheContain(string hash);
        BitmapImage GetFromCache(string hash);
        Task SendAvatarRequest(IEntityIdentifier id);
        event EventHandler<AvatarChangedEventArgs> AvatarChange;
    }
}
