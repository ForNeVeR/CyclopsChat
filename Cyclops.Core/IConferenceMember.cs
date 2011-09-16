using System.Windows.Media.Imaging;

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

        IEntityIdentifier ConferenceUserId { get; }
        IEntityIdentifier RealUserId { get; }
    }
}