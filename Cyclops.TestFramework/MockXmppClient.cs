using Cyclops.Core;
using Cyclops.Xmpp.SharpXmpp.Client;
using SharpXMPP.XMPP.Client.Elements;

namespace Cyclops.TestFramework;

public class MockXmppClient : SharpXmppClient
{
    public MockXmppClient(ILogger logger) : base(logger) {}

    public void FirePresence(XMPPPresence presence)
    {
        OnPresence(null, presence);
    }
}
