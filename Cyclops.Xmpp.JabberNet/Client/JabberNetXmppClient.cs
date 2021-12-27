using System.Xml;
using Cyclops.Xmpp.Client;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.JabberNet.Elements;
using Cyclops.Xmpp.JabberNet.Protocol;
using Cyclops.Xmpp.Protocol;
using jabber;
using jabber.client;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Cyclops.Xmpp.JabberNet.Client;

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

    public async Task<IIq> SendCaptchaAnswer(IEntityIdentifier conferenceId, string challenge, string answer)
    {
        var conferenceJid = (JID)conferenceId;
        var iq = new TypedIQ<CaptchaAnswer>(client.Document)
        {
            To = conferenceJid.BareJID,
            Type = IQType.set,
            Instruction =
            {
                CaptchaAnswerX = new CaptchaAnswerX(client.Document)
            }
        };

        iq.Instruction.CaptchaAnswerX.FillAnswer(answer, conferenceJid, challenge);

        var response = await SendIq(iq);
        return response.Wrap();
    }

    public async Task<Vcard> GetVCard(IEntityIdentifier jid)
    {
        var vCardIq = new VCardIQ(client.Document)
        {
            To = (JID)jid,
            Type = IQType.get
        };
        var iq = await SendIq(vCardIq);
        return iq.ToVCard();
    }
}
