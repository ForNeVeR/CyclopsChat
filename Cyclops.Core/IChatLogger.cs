using Cyclops.Xmpp.Data;

namespace Cyclops.Core
{
    public interface IChatLogger
    {
        void AddRecord(IEntityIdentifier id, string message, bool isPrivate = false);
    }
}
