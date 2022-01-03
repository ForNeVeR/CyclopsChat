using System.Xml.Linq;
using Cyclops.Xmpp.Protocol;
using SharpXMPP.XMPP.Client.Elements;
using Namespaces = SharpXMPP.Namespaces;

namespace Cyclops.Xmpp.SharpXmpp.Protocol;

public static class MessageEx
{
    private class XmppMessage : IMessage
    {
        private readonly XMPPMessage original;
        public XmppMessage(XMPPMessage original)
        {
            this.original = original;
        }

        public Jid? From => original.From.Map();
        public Jid? To => original.To.Map();
        public string? Subject => original.Element(XNamespace.Get(Namespaces.JabberClient) + Elements.Subject)?.Value;
        public string? Body => original.Text;
        public IError? Error => original.Element(XNamespace.Get(Namespaces.JabberClient) + Elements.Error)?.WrapAsError();
        public MessageType Type => original.GetMessageType();
    }

    private class XmppError : IError
    {
        public int Code => throw new NotImplementedException();
        public string? Message => throw new NotImplementedException();
    }

    public static IMessage Wrap(this XMPPMessage message) => new XmppMessage(message);

    private static MessageType GetMessageType(this XMPPMessage message)
    {
        var typeName = message.Attribute("type")?.Value;
        return typeName switch
        {
            "chat" => MessageType.Chat,
            "error" => MessageType.Error,
            "groupchat" => MessageType.GroupChat,
            "headline" => MessageType.Headline,
            "normal" => MessageType.Normal,
            _ => throw new NotSupportedException($"Unknown message type: {typeName}.")
        };
    }

    private static IError WrapAsError(this XElement error) => new XmppError();
}
