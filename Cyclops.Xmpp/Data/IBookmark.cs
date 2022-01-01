using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Data;

public interface IBookmark
{
    Jid ConferenceJid { get; }
    string Nick { get; }
    string Name { get; }
    bool AutoJoin { get; }
}
