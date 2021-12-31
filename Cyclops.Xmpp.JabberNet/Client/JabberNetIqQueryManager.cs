using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol.IqQueries;
using jabber.client;
using jabber.protocol.client;
using jabber.protocol.iq;
using Version = jabber.protocol.iq.Version;

namespace Cyclops.Xmpp.JabberNet.Client;

internal class JabberNetIqQueryManager : IIqQueryManager
{
    public JabberNetIqQueryManager(JabberClient client)
    {
        client.OnIQ += (_, iq) => ProcessIq(iq);
    }

    public event EventHandler<ITimeIq>? TimeQueried;
    public event EventHandler<ILastIq>? LastQueried;
    public event EventHandler<IVersionIq>? VersionQueried;

    private void ProcessIq(IQ iq)
    {
        if (iq.Type != IQType.get)
            return;

        switch (iq.Query)
        {
            case Time:
                TimeQueried?.Invoke(this, iq.WrapTime());
                break;
            case Last:
                LastQueried?.Invoke(this, iq.WrapLast());
                break;
            case Version:
                VersionQueried?.Invoke(this, iq.WrapVersion());
                break;
        }
    }
}
