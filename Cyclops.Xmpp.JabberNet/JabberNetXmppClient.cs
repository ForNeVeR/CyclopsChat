using System.Xml;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using jabber;
using jabber.client;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Cyclops.Xmpp.JabberNet;

public sealed class JabberNetXmppClient : IXmppClient, IDisposable
{
    private readonly JabberClient client;

    public JabberNetXmppClient(JabberClient client)
    {
        this.client = client;
        InitializeEvents();
    }

    public void Dispose() => client.Dispose();

    public event EventHandler? Connect;
    public event EventHandler<string>? ReadRawMessage;
    public event EventHandler<string>? WriteRawMessage;
    public event EventHandler<Exception>? Error;

    public event EventHandler<IPresence>? Presence;

    private void InitializeEvents()
    {
        client.OnConnect += delegate { Connect?.Invoke(this, null); };
        client.OnReadText += (_, text) => ReadRawMessage?.Invoke(this, text);
        client.OnWriteText += (_, text) => WriteRawMessage?.Invoke(this, text);
        client.OnError += (_, error) => Error?.Invoke(this, error);

        client.OnPresence += (_, presence) => Presence?.Invoke(this, presence.Wrap());
    }

    public void SendElement(XmlElement element)
    {
        client.Write(element);
    }

    public Task<Vcard> GetVCard(IEntityIdentifier jid)
    {
        var result = new TaskCompletionSource<Vcard>();

        var vcardIq = new VCardIQ(client.Document)
        {
            To = (JID)jid,
            Type = IQType.get
        };

        client.Tracker.BeginIQ(vcardIq, (_, iq, _) =>
        {
            try
            {
                result.SetResult(iq.ToVCard());
            }
            catch (Exception ex)
            {
                result.SetException(ex);
            }
        }, null);

        return result.Task;
    }
}
