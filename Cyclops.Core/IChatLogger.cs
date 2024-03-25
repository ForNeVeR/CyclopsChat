using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core
{
    public interface IChatLogger
    {
        void AddRecord(Jid? id, string message, bool isPrivate = false);
    }
}
