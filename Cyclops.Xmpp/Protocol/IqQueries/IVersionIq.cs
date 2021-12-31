using Cyclops.Xmpp.Data;

namespace Cyclops.Xmpp.Protocol.IqQueries;

public interface IVersionIq : ITypedIq<IVersionIq>
{
    public ClientInfo ClientInfo { get; set; }
}
