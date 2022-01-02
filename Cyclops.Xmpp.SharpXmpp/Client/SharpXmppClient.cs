using System.Xml;
using Cyclops.Core;
using Cyclops.Core.Helpers;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Protocol;
using SharpXMPP;
using SharpXMPP.XMPP;
using SharpXMPP.XMPP.Client.Elements;

namespace Cyclops.Xmpp.SharpXmpp.Client;

public class SharpXmppClient : IXmppClient
{
    private readonly ILogger logger;
    private XmppClient? client;

    public SharpXmppClient(ILogger logger)
    {
        this.logger = logger;
        IqQueryManager = new SharpXmppIqQueryManager();
        BookmarkManager = new SharpXmppBookmarkManager();
        ConferenceManager = new SharpXmppConferenceManager();
    }

    public void Dispose()
    {
    }

    public IIqQueryManager IqQueryManager { get; }
    public IBookmarkManager BookmarkManager { get; }
    public IConferenceManager ConferenceManager { get; }

    public event EventHandler? Connected;
    public event EventHandler? Disconnected
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
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
    public event EventHandler<Exception>? Error
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler? StreamError
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler? Authenticated
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler? AuthenticationError;
    public event EventHandler<IPresence>? Presence;
    public event EventHandler? RoomMessage
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<IMessage>? Message
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }

    public bool IsAuthenticated => throw new NotImplementedException();

    public void Connect(string server, string host, string user, string password, int port, string resource)
    {
        if (client != null)
        {
            UnsubscribeFromEvents(client);
            client?.Dispose();
        }

        client = new XmppClient(new JID($"{user}@{server}"), password);
        SubscribeToEvents(client);

        DoConnect().NoAwait(logger);
        async Task DoConnect()
        {
            try
            {
                await client.ConnectAsync();
                Connected?.Invoke(this, null);
            }
            catch (Exception)
            {
                AuthenticationError?.Invoke(this, null);
                throw;
            }
        }
    }

    private void SubscribeToEvents(XmppClient currentClient)
    {
        currentClient.Presence += OnPresence;
    }

    private void UnsubscribeFromEvents(XmppClient currentClient)
    {
        currentClient.Presence -= OnPresence;
    }

    private void OnPresence(XmppConnection _, XMPPPresence presence) => Presence?.Invoke(this, presence.Wrap());

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
