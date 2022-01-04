using System.Drawing;
using System.Globalization;
using System.Xml;
using System.Xml.Linq;
using Cyclops.Core;
using Cyclops.Core.Helpers;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol;
using Cyclops.Xmpp.SharpXmpp.Errors;
using Cyclops.Xmpp.SharpXmpp.Protocol;
using SharpXMPP;
using SharpXMPP.XMPP;
using SharpXMPP.XMPP.Client.Elements;
using Namespaces = Cyclops.Xmpp.Protocol.Namespaces;

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
        ConferenceManager = new SharpXmppConferenceManager(logger, this);
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
    public event EventHandler<string>? ReadRawMessage;
    public event EventHandler<string>? WriteRawMessage;
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

        currentClient = new XmppClient(new JID($"{user}@{server}"), password, autoPresence: false);
        SubscribeToEvents(currentClient);
        iqQueryManager.IqManager = currentClient.IqManager;
        bookmarkManager.Connection = currentClient;

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
        logger.LogVerbose("{0}\n{1}", e.IsInput ? "IN:" : "OUT:", e.Stanza);

        if (e.IsInput)
            ReadRawMessage?.Invoke(this, e.Stanza.ToString());
        else
            WriteRawMessage?.Invoke(this, e.Stanza.ToString());
        switch (e)
        {
            case { IsInput: true, Stanza.Name: { NamespaceName: SharpXMPP.Namespaces.Streams, LocalName: Elements.Error } }:
                StreamError?.Invoke(this, null);
                break;
        }
    }

    private void OnSignedIn(XmppConnection sender, SignedInArgs e)
    {
        Authenticated?.Invoke(this, null);
        sender.Send(new XMPPPresence());
    }

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

    public void SendPresence(PresenceDetails presenceDetails)
    {
        var presence = new XMPPPresence();
        if (presenceDetails.Type != null)
            presence.SetAttributeValue("type", presenceDetails.Type.Value.Map());
        if (presenceDetails.To != null)
            presence.To = presenceDetails.To.Value.Map();

        if (presenceDetails.StatusText != null)
        {
            var status = presence.GetOrCreateChildElement(
                XNamespace.Get(SharpXMPP.Namespaces.JabberClient) + "status");
            status.Value = presenceDetails.StatusText;
        }

        if (presenceDetails.StatusType != null)
        {
            var show = presence.GetOrCreateChildElement(XNamespace.Get(SharpXMPP.Namespaces.JabberClient) + "show");
            show.Value = presenceDetails.StatusType.Value.Map();
        }

        if (presenceDetails.PhotoHash != null)
        {
            var x = presence.GetOrCreateChildElement(
                XNamespace.Get(Namespaces.VCardTempXUpdate) + Elements.X);
            var photo = x.GetOrCreateChildElement(
                XNamespace.Get(Namespaces.VCardTempXUpdate) + Elements.Photo);
            photo.Value = presenceDetails.PhotoHash;
        }

        if (presenceDetails.Priority != null)
        {
            var priority = presence.GetOrCreateChildElement(
                XNamespace.Get(SharpXMPP.Namespaces.JabberClient) + "priority");
            priority.Value = presenceDetails.Priority.Value.ToString(CultureInfo.InvariantCulture);
        }

        currentClient!.Send(presence);
    }

    internal void SendPresence(XMPPPresence presence)
    {
        presence.GetOrCreateAttribute(Attributes.From).Value ??= currentClient!.Jid?.FullJid;

        currentClient!.Send(presence);
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
        var iq = new XMPPIq(XMPPIq.IqTypes.get)
        {
            To = jid.Map()
        };
        iq.GetOrCreateChildElement(XNamespace.Get(Namespaces.VCardTemp) + Elements.VCard);

        var result = new TaskCompletionSource<VCard>();
        currentClient!.Query(iq, response =>
        {
            try
            {
                var vCardElement = response.Element(XNamespace.Get(Namespaces.VCardTemp) + Elements.VCard);
                var photoElement = vCardElement?.Element(XNamespace.Get(Namespaces.VCardTemp) + Elements.VCardPhoto);
                var fullNameElement = vCardElement?.Element(XNamespace.Get(Namespaces.VCardTemp) + Elements.VCardFn);
                var emailElement = vCardElement?.Elements(XNamespace.Get(Namespaces.VCardTemp) + Elements.VCardEmail)
                    .FirstOrDefault();
                var birthDateElement = vCardElement?.Element(XNamespace.Get(Namespaces.VCardTemp) + Elements.VCardBDay);
                var nicknameElement = vCardElement?.Element(XNamespace.Get(Namespaces.VCardTemp) + Elements.VCardNickname);
                var descriptionElement = vCardElement?.Element(XNamespace.Get(Namespaces.VCardTemp) + Elements.VCardDesc);

                var photo = photoElement == null ? null : ReadPhoto(photoElement);
                var birthDate =
                    birthDateElement == null ? null
                    : DateTime.TryParse(birthDateElement.Value, CultureInfo.InvariantCulture,
                        DateTimeStyles.RoundtripKind, out var bd) ? (DateTime?)bd : null;

                result.SetResult(new VCard
                {
                    Photo = photo,
                    FullName = fullNameElement?.Value,
                    Email = emailElement?.Value,
                    Birthday = birthDate,
                    Nick = nicknameElement?.Value,
                    Comments = descriptionElement?.Value
                });
            }
            catch (Exception ex)
            {
                result.SetException(ex);
            }
        });
        return result.Task;

        Image? ReadPhoto(XElement photo)
        {
            var binVal = photo.Element(XNamespace.Get(Namespaces.VCardTemp) + Elements.VCardPhotoBinVal);
            if (binVal == null) return null;
            try
            {
                var bytes = Convert.FromBase64String(binVal.Value);
                if (bytes.Length == 0) return null;
                using var stream = new MemoryStream(bytes);
                return Image.FromStream(stream);
            }
            catch (Exception ex)
            {
                logger.LogError($"Error while processing avatar of user {iq.From}.", ex);
                return null;
            }
        }
    }

    public Task<IIq> UpdateVCard(VCard vCard)
    {
        throw new NotImplementedException();
    }

    public Task<ClientInfo?> GetClientInfo(Jid jid)
    {
        var iq = new XMPPIq(XMPPIq.IqTypes.get);
        iq.GetOrCreateChildElement(XNamespace.Get(Namespaces.Version) + Elements.Query);

        var result = new TaskCompletionSource<ClientInfo?>();
        currentClient!.Query(iq, response =>
        {
            try
            {
                var query = response.Element(XNamespace.Get(Namespaces.Version) + Elements.Query);
                var name = query?.Element(XNamespace.Get(Namespaces.Version) + Elements.Name);
                var version = query?.Element(XNamespace.Get(Namespaces.Version) + Elements.Version);
                var os = query?.Element(XNamespace.Get(Namespaces.Version) + Elements.Os);

                var clientInfo = new ClientInfo(os?.Value, version?.Value, name?.Value);
                result.SetResult(clientInfo);
            }
            catch (Exception ex)
            {
                result.SetException(ex);
            }
        });
        return result.Task;
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
