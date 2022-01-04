using System.Xml;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Client;

public interface IXmppClient : IDisposable
{
    IIqQueryManager IqQueryManager { get; }
    IBookmarkManager BookmarkManager { get; }
    IConferenceManager ConferenceManager { get; }

    event EventHandler Connected;
    event EventHandler Disconnected;
    event EventHandler<string> ReadRawMessage;
    event EventHandler<string> WriteRawMessage;
    event EventHandler<Exception> Error;
    event EventHandler StreamError;

    event EventHandler Authenticated;
    event EventHandler AuthenticationError;

    event EventHandler<IPresence> Presence;
    event EventHandler RoomMessage;
    event EventHandler<IMessage> Message;

    bool IsAuthenticated { get; }

    void Connect(string server, string host, string user, string password, int port, string resource);
    void Disconnect();

    void SendElement(XmlElement element);

    void SendPresence(PresenceDetails presenceDetails);
    void SendIq(IIq iq);
    void SendMessage(MessageType type, Jid target, string body);

    Task<IIq> SendCaptchaAnswer(Jid roomId, string challenge, string answer);

    Task<VCard> GetVCard(Jid jid);
    Task<IIq> UpdateVCard(VCard vCard);

    Task<ClientInfo?> GetClientInfo(Jid jid);

    Task<IDiscoNode?> DiscoverItems(Jid jid, string node);
}
