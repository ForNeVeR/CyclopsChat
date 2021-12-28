using System.Windows.Media.Imaging;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core
{
    public interface IConferenceMember : ISessionHolder
    {
        string Nick { get; }
        bool IsModer { get; }
        bool IsMe { get; }
        string StatusText { get; }
        string StatusType { get; }
        bool IsSubscribed { get; }

        Role Role { get; }
        ClientInfo ClientInfo { get; }
        BitmapImage AvatarUrl { get; }

        Jid ConferenceUserId { get; }
        Jid? RealUserId { get; }
    }
}
