using System.Xml.Linq;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Protocol.IqQueries;
using Cyclops.Xmpp.SharpXmpp.Protocol;
using SharpXMPP;
using SharpXMPP.XMPP.Client;
using SharpXMPP.XMPP.Client.Elements;

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

    private IqManager? iqManager;
    internal IqManager IqManager
    {
        set
        {
            if (iqManager != null)
                throw new NotSupportedException(
                    $"Reinitialization of {nameof(SharpXmppIqQueryManager)} is not supported.");

            iqManager = value;
            RegisterPayloadHandler(iqManager);
        }
    }

    private void RegisterPayloadHandler(IqManager currentIqManager)
    {
        currentIqManager.PayloadHandlers.Add(new LambdaPayloadHandler(iq =>
        {
            if (iq.IqType != XMPPIq.IqTypes.get) return false;

            var timeQuery = iq.Element(XNamespace.Get(Namespaces.Time) + Elements.Query);
            if (timeQuery != null)
            {
                TimeQueried?.Invoke(this, iq.WrapTime());
                return true;
            }

            return false;
        }));
    }

    public event EventHandler<ITimeIq>? TimeQueried;
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
