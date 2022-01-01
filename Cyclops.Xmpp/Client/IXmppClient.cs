using System.Xml;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Xmpp.Client;

public interface IXmppClient
{
    IIqQueryManager IqQueryManager { get; }
    IBookmarkManager BookmarkManager { get; }
    IConferenceManager ConferenceManager { get; }

    event EventHandler Connect;
    event EventHandler<string> ReadRawMessage;
    event EventHandler<string> WriteRawMessage;
    event EventHandler<Exception> Error;

    event EventHandler<IPresence> Presence;
    event EventHandler RoomMessage;

    void SendElement(XmlElement element);

    void SendPresence(PresenceDetails presence);
    void SendIq(IIq iq);

    Task<IIq> SendCaptchaAnswer(Jid mucId, string challenge, string answer);

    Task<VCard> GetVCard(Jid jid);
    Task<IIq> UpdateVCard(VCard vCard);

    Task<ClientInfo?> GetClientInfo(Jid jid);

    IRoom GetRoom(Jid roomJid);

    Task<IDiscoNode?> DiscoverItems(Jid jid, string node);
    Task<IDiscoNode?> DiscoverItemsWithFeature(string featureUri);
}
