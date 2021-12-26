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

    public event Action? Connect;
    public event EventHandler<string>? ReadRawMessage;
    public event EventHandler<string>? WriteRawMessage;
    public event EventHandler<Exception>? Error;

    private void InitializeEvents()
    {
        client.OnConnect += delegate { Connect?.Invoke(); };
        client.OnReadText += (sender, text) => ReadRawMessage?.Invoke(sender, text);
        client.OnWriteText += (sender, text) => WriteRawMessage?.Invoke(sender, text);
        client.OnError += (sender, error) => Error?.Invoke(sender, error);
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
