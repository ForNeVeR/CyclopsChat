using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Protocol.IqQueries;

namespace Cyclops.Xmpp.SharpXmpp.Client;

public class SharpXmppIqQueryManager : IIqQueryManager
{
    public event EventHandler<ITimeIq>? TimeQueried
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<ILastIq>? LastQueried
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<IVersionIq>? VersionQueried
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
}
