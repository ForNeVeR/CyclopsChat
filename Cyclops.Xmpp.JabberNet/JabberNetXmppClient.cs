using System.Xml;
using jabber.client;

namespace Cyclops.Xmpp.JabberNet;

public sealed class JabberNetXmppClient : IXmppClient, IDisposable
{
    private readonly JabberClient client;

    public JabberNetXmppClient(JabberClient client)
    {
        this.client = client;
        InitializeEvents();
    }

    public event EventHandler? Connect;
    public event EventHandler<string>? ReadRawMessage;
    public event EventHandler<string>? WriteRawMessage;
    public event EventHandler<Exception>? Error;

    private void InitializeEvents()
    {
        client.OnConnect += delegate { Connect?.Invoke(this, null); };
        client.OnReadText += (sender, text) => ReadRawMessage?.Invoke(this, text);
        client.OnWriteText += (sender, text) => WriteRawMessage?.Invoke(this, text);
        client.OnError += (sender, error) => Error?.Invoke(this, error);
    }

    public void SendElement(XmlElement element)
    {
        client.Write(element);
    }

    public void Dispose()
    {
        client.Dispose();
    }
}
