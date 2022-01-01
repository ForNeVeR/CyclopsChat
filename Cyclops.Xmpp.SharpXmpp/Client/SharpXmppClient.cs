using System.Xml;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.SharpXmpp.Client;

public class SharpXmppClient : IXmppClient
{
    public SharpXmppClient()
    {
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

    public event EventHandler? Connected
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
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
    public event EventHandler? AuthenticationError
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
    public event EventHandler<IPresence>? Presence
    {
        add => throw new NotImplementedException();
        remove => throw new NotImplementedException();
    }
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
        throw new NotImplementedException();
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
