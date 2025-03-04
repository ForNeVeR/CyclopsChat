using System.Drawing;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core;

public interface IConferenceMember : ISessionHolder
{
    string Nick { get; }
    bool IsModer { get; }
    bool IsMe { get; }
    string StatusText { get; }
    string StatusType { get; }
    bool IsSubscribed { get; set; }

    Role Role { get; }
    ClientInfo ClientInfo { get; }
    Image AvatarUrl { get; }

    Jid ConferenceUserId { get; }
    Jid? RealUserId { get; }
}
