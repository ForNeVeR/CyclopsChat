using System.Xml.Linq;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.Protocol.IqQueries;
using Cyclops.Xmpp.SharpXmpp.Protocol;
using SharpXMPP;
using SharpXMPP.XMPP.Client;
using SharpXMPP.XMPP.Client.Elements;
using Namespaces = Cyclops.Xmpp.Protocol.Namespaces;

namespace Cyclops.Xmpp.SharpXmpp.Client;

public class SharpXmppIqQueryManager : IIqQueryManager
{
    private class LambdaPayloadHandler : PayloadHandler
    {
        private readonly Func<XMPPIq, bool> handler;
        public LambdaPayloadHandler(Func<XMPPIq, bool> handler)
        {
            this.handler = handler;
        }

        public override bool Handle(XmppConnection _, XMPPIq element) => handler(element);
    }

    private readonly LambdaPayloadHandler payloadHandler;
    private IqManager? iqManager;

    public SharpXmppIqQueryManager()
    {
        payloadHandler = new LambdaPayloadHandler(iq =>
        {
            if (iq.IqType != XMPPIq.IqTypes.get) return false;

            var timeQuery = iq.Element(XNamespace.Get(Namespaces.Time) + Elements.Query);
            if (timeQuery != null)
            {
                TimeQueried?.Invoke(this, iq.WrapTime());
                return true;
            }

            var lastActivityQuery = iq.Element(XNamespace.Get(Namespaces.Last) + Elements.Query);
            if (lastActivityQuery != null)
            {
                LastActivityQueried?.Invoke(this, iq.WrapLast());
                return true;
            }

            var versionQuery = iq.Element(XNamespace.Get(Namespaces.Version) + Elements.Query);
            if (versionQuery != null)
            {
                VersionQueried?.Invoke(this, iq.WrapVersion());
                return true;
            }

            return false;
        });
    }

    internal IqManager IqManager
    {
        set
        {
            if (iqManager != null)
                UnregisterPayloadHandler(iqManager);

            iqManager = value;
            RegisterPayloadHandler(iqManager);
        }
    }

    private void UnregisterPayloadHandler(IqManager oldIqManager)
    {
        oldIqManager.PayloadHandlers.Remove(payloadHandler);
    }

    private void RegisterPayloadHandler(IqManager currentIqManager)
    {
        currentIqManager.PayloadHandlers.Add(payloadHandler);
    }

    public event EventHandler<ITimeIq>? TimeQueried;
    public event EventHandler<ILastIq>? LastActivityQueried;
    public event EventHandler<IVersionIq>? VersionQueried;
}
