using System.Xml;
using Cyclops.Core;
using Cyclops.Core.Helpers;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Errors;
using Cyclops.Xmpp.SharpXmpp.Protocol;
using SharpXMPP;
using SharpXMPP.XMPP;
using SharpXMPP.XMPP.Client.Elements;
using Namespaces = SharpXMPP.Namespaces;

namespace Cyclops.Xmpp.SharpXmpp.Client;

public class SharpXmppClient : IXmppClient
{
    private readonly ILogger logger;
    private readonly SharpXmppIqQueryManager iqQueryManager;
    private readonly SharpXmppBookmarkManager bookmarkManager;

    private XmppClient? currentClient;

    public SharpXmppClient(ILogger logger)
    {
        this.logger = logger;
        iqQueryManager = new SharpXmppIqQueryManager();
        bookmarkManager = new SharpXmppBookmarkManager(logger);
        ConferenceManager = new SharpXmppConferenceManager();
    }

    public void Dispose()
    {
        currentClient?.Dispose();
    }

    public IIqQueryManager IqQueryManager => iqQueryManager;
    public IBookmarkManager BookmarkManager => bookmarkManager;
    public IConferenceManager ConferenceManager { get; }

    public event EventHandler? Connected;
    public event EventHandler? Disconnected;
    public event EventHandler<string>? ReadRawMessage
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<string>? WriteRawMessage
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<Exception>? Error;
    public event EventHandler? StreamError;
    public event EventHandler? Authenticated;
    public event EventHandler? AuthenticationError;
    public event EventHandler<IPresence>? Presence;
    public event EventHandler? RoomMessage;
    public event EventHandler<IMessage>? Message;

    public bool IsAuthenticated => throw new NotImplementedException();

    public void Connect(string server, string host, string user, string password, int port, string resource)
    {
        if (currentClient != null)
        {
            UnsubscribeFromEvents(currentClient);
            currentClient?.Dispose();
        }

        currentClient = new XmppClient(new JID($"{user}@{server}"), password);
        SubscribeToEvents(currentClient);
        iqQueryManager.IqManager = currentClient.IqManager;
        bookmarkManager.Connection = currentClient;
        bookmarkManager.BookmarkManager = currentClient.BookmarkManager;

        DoConnect().NoAwait(logger);
        async Task DoConnect()
        {
            try
            {
                await currentClient.ConnectAsync();
            }
            catch (Exception ex)
            {
                AuthenticationError?.Invoke(this, null);
                Error?.Invoke(this, ex);
                throw;
            }
        }
    }

    private void SubscribeToEvents(XmppConnection connection)
    {
        connection.StreamStart += OnStreamStart;
        connection.Element += OnElement;
        connection.SignedIn += OnSignedIn;
        connection.Presence += OnPresence;
        connection.Message += OnMessage;
        connection.ConnectionFailed += OnConnectionFailed;
    }

    private void UnsubscribeFromEvents(XmppConnection connection)
    {
        connection.StreamStart -= OnStreamStart;
        connection.Element -= OnElement;
        connection.SignedIn -= OnSignedIn;
        connection.Presence -= OnPresence;
        connection.Message -= OnMessage;
        connection.ConnectionFailed -= OnConnectionFailed;
    }

    private void OnStreamStart(XmppConnection _, string __) => Connected?.Invoke(this, null);

    private void OnElement(XmppConnection _, ElementArgs e)
    {
        switch (e)
        {
            case { IsInput: true, Stanza.Name: { NamespaceName: Namespaces.Streams, LocalName: Elements.Error } }:
                StreamError?.Invoke(this, null);
                break;
        }
    }

    private void OnSignedIn(XmppConnection sender, SignedInArgs e) => Authenticated?.Invoke(this, null);

    private void OnPresence(XmppConnection _, XMPPPresence presence) => Presence?.Invoke(this, presence.Wrap());

    private void OnMessage(XmppConnection _, XMPPMessage message)
    {
        var wrapped = message.Wrap();
        if (wrapped.Type == MessageType.GroupChat)
            RoomMessage?.Invoke(this, null);

        Message?.Invoke(this, message.Wrap());
    }

    private void OnConnectionFailed(XmppConnection _, ConnFailedArgs e)
    {
        // NOTE: ideally, we shouldn't log the exception here since we're able to propagate it further. But,
        // unfortunately, in such case we may lose e.Message (since only e.Exception gets propagated). So we have to log
        // e.Message here. And if we're doing this, then it would be odd to only log e.Message without e.Exception.
        if (e.Message != null)
            logger.LogError($"Connection failed: {e.Message}.", e.Exception);

        Disconnected?.Invoke(this, null);
        Error?.Invoke(this, e.Exception ?? new MessageOnlyException(e.Message));
    }

    public void Disconnect()
    {
        throw new NotImplementedException();
    }

    public void SendElement(XmlElement element)
    {
        throw new NotImplementedException();
    }

    public void SendPresence(PresenceDetails presence)
    {
        throw new NotImplementedException();
    }

    public void SendIq(IIq iq)
    {
        throw new NotImplementedException();
    }

    public void SendMessage(MessageType type, Jid target, string body)
    {
        throw new NotImplementedException();
    }

    public Task<IIq> SendCaptchaAnswer(Jid mucId, string challenge, string answer)
    {
        throw new NotImplementedException();
    }

    public Task<VCard> GetVCard(Jid jid)
    {
        throw new NotImplementedException();
    }

    public Task<IIq> UpdateVCard(VCard vCard)
    {
        throw new NotImplementedException();
    }

    public Task<ClientInfo?> GetClientInfo(Jid jid)
    {
        throw new NotImplementedException();
    }

    public IRoom GetRoom(Jid roomJid)
    {
        throw new NotImplementedException();
    }

    public Task<IDiscoNode?> DiscoverItems(Jid jid, string node)
    {
        throw new NotImplementedException();
    }

    public Task<IDiscoNode?> DiscoverItemsWithFeature(string featureUri)
    {
        throw new NotImplementedException();
    }
}
