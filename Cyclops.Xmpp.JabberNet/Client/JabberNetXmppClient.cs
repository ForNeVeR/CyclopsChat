using System.ComponentModel;
using System.Globalization;
using System.Xml;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Data.Rooms;
using Cyclops.Xmpp.JabberNet.Data;
using Cyclops.Xmpp.JabberNet.Data.Rooms;
using Cyclops.Xmpp.JabberNet.Elements;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;
using jabber.client;
using jabber.connection;
using jabber.protocol.client;
using jabber.protocol.iq;
using jabber.protocol.stream;
using MessageType = Cyclops.Xmpp.Protocol.MessageType;
using VCard = Cyclops.Xmpp.Data.VCard;

namespace Cyclops.Xmpp.JabberNet.Client;

public sealed class JabberNetXmppClient : IXmppClient
{
    private readonly JabberClient client;
    private readonly ConferenceManager conferenceManager;
    private readonly DiscoManager discoManager;

    public IIqQueryManager IqQueryManager { get; }
    public IBookmarkManager BookmarkManager { get; }
    public IConferenceManager ConferenceManager { get; }

    public JabberNetXmppClient(ISynchronizeInvoke synchronizer)
    {
        client = new JabberClient
        {
            InvokeControl = synchronizer,
            AutoReconnect = -1,
            AutoPresence = true,
            AutoRoster = false,
            [Options.SASL_MECHANISMS] = MechanismType.DIGEST_MD5,
            KeepAlive = 20F
        };
        conferenceManager = new ConferenceManager { Stream = client };
        discoManager = new DiscoManager { Stream = client };

        IqQueryManager = new JabberNetIqQueryManager(client);
        BookmarkManager = new JabberNetBookmarkManager(new BookmarkManager
        {
            Stream = client,
            AutoPrivate = false,
            ConferenceManager = conferenceManager
        });
        ConferenceManager = new JabberNetConferenceManager(conferenceManager);

        InitializeEvents();
    }

    public void Dispose() => client.Dispose();

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

    private void InitializeEvents()
    {
        client.OnConnect += delegate { Connected?.Invoke(this, null); };
        client.OnDisconnect += delegate { Disconnected?.Invoke(this, null); };
        client.OnReadText += (_, text) => ReadRawMessage?.Invoke(this, text);
        client.OnWriteText += (_, text) => WriteRawMessage?.Invoke(this, text);
        client.OnError += (_, error) => Error?.Invoke(this, error);
        client.OnStreamError += delegate { StreamError?.Invoke(this, null); };

        client.OnAuthenticate += delegate { Authenticated?.Invoke(this, null); };
        client.OnAuthError += delegate { AuthenticationError?.Invoke(this, null); };

        client.OnPresence += (_, presence) => Presence?.Invoke(this, presence.Wrap());
        conferenceManager.OnRoomMessage += (_, _) => RoomMessage?.Invoke(this, null);
        client.OnMessage += (_, message) => Message?.Invoke(this, message.Wrap());
    }

    public bool IsAuthenticated => client.IsAuthenticated;

    public void Connect(string server, string host, string user, string password, int port, string resource)
    {
        client.Server = server;
        client.NetworkHost = host;
        client.User = user;
        client.Password = password;
        client.Port = port;
        client.Resource = resource;
        client.Connect();
    }

    public void Disconnect() => client.Close(true);

    public void SendElement(XmlElement element)
    {
        client.Write(element);
    }

    public void SendPresence(PresenceDetails presence)
    {
        var presenceToSend = new Presence(client.Document);
        if (presence.Type != null)
            presenceToSend.Type = presence.Type.Value.Map();
        if (presence.To != null)
            presenceToSend.To = presence.To.Value.Map();
        if (presence.StatusText != null)
            presenceToSend.Status = presence.StatusText;
        if (presence.StatusType != null)
            presenceToSend.Show = presence.StatusType?.Map();
        if (presence.PhotoHash != null)
            presenceToSend.AddChild(new PhotoX(client.Document) { PhotoHash = presence.PhotoHash });
        if (presence.Priority != null)
            presenceToSend.Priority = presence.Priority.Value.ToString(CultureInfo.InvariantCulture);

        client.Write(presenceToSend);
    }

    private Task<IQ> SendIq(IQ request)
    {
        var result = new TaskCompletionSource<IQ>();

        client.Tracker.BeginIQ(request, (_, response, _) =>
        {
            try
            {
                result.SetResult(response);
            }
            catch (Exception ex)
            {
                result.SetException(ex);
            }
        }, null);

        return result.Task;
    }

    public void SendIq(IIq iq)
    {
        var protocolIq = iq.Unwrap();
        client.Write(protocolIq);
    }

    public void SendMessage(MessageType type, Jid target, string body) =>
        client.Message(type.Map(), target.ToString(), body);

    public async Task<IIq> SendCaptchaAnswer(Jid conferenceId, string challenge, string answer)
    {
        var iq = new TypedIQ<CaptchaAnswer>(client.Document)
        {
            To = conferenceId.Bare.Map(),
            Type = IQType.set,
        };

        iq.Instruction!.CaptchaAnswerX = new CaptchaAnswerX(client.Document);
        iq.Instruction.CaptchaAnswerX.FillAnswer(answer, conferenceId.Map(), challenge);

        var response = await SendIq(iq);
        return response.Wrap();
    }

    public async Task<VCard> GetVCard(Jid jid)
    {
        var vCardIq = new VCardIQ(client.Document)
        {
            To = jid.Map(),
            Type = IQType.get
        };
        var iq = await SendIq(vCardIq);
        return iq.ToVCard();
    }

    public async Task<IIq> UpdateVCard(VCard vCard)
    {
        var iq = new VCardIQ(client.Document)
        {
            Type = IQType.set
        };
        var photo = iq.VCard.Photo = new jabber.protocol.iq.VCard.VPhoto(client.Document);
        if (vCard.Photo != null)
            photo.ImageType = vCard.Photo.RawFormat;

        photo.Image = vCard.Photo;
        iq.VCard.Photo = photo;
        iq.VCard.Description = vCard.Comments;
        iq.VCard.Birthday = vCard.Birthday;
        iq.VCard.FullName = vCard.FullName;

        var response = await SendIq(iq);
        return response.Wrap();
    }

    public async Task<ClientInfo?> GetClientInfo(Jid jid)
    {
        var versionIq = new VersionIQ(client.Document) { To = jid.Map(), Type = IQType.get };
        // HACK: Most clients' answers aren't recognized as VersionIQ by Jabber-Net, so let's not break on cast failure.
        var response = await SendIq(versionIq) as VersionIQ;
        var versionInfo = response?.Instruction;
        return versionInfo == null ? null : new ClientInfo(versionInfo.OS, versionInfo.Ver, versionInfo.EntityName);
    }

    public IRoom GetRoom(Jid roomJid) => conferenceManager.GetRoom(roomJid.Map()).Wrap();

    public Task<IDiscoNode?> DiscoverItems(Jid jid, string node)
    {
        var task = new TaskCompletionSource<IDiscoNode?>();
        discoManager.BeginGetItems(jid.Map(), node, (_, discoNode, _) =>
        {
            try
            {
                task.SetResult(discoNode.Wrap());
            }
            catch (Exception ex)
            {
                task.SetException(ex);
            }
        }, null, false);
        return task.Task;
    }

    public Task<IDiscoNode?> DiscoverItemsWithFeature(string featureUri)
    {
        var task = new TaskCompletionSource<IDiscoNode?>();
        discoManager.BeginFindServiceWithFeature(featureUri, (_, discoNode, _) =>
        {
            try
            {
                task.SetResult(discoNode.Wrap());
            }
            catch (Exception ex)
            {
                task.SetException(ex);
            }
        }, null);
        return task.Task;
    }
}
