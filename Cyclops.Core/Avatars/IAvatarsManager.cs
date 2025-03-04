using System;
using System.Threading.Tasks;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.Avatars;

public interface IAvatarsManager : ISessionHolder
{
    bool DoesCacheContain(string hash);
    byte[] GetFromCache(string hash);
    Task SendAvatarRequest(Jid id);
    event EventHandler<AvatarChangedEventArgs> AvatarChange;
}
