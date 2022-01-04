using System.Xml.Linq;
using Cyclops.Xmpp.Protocol;
using SharpXMPP.XMPP.Client.Elements;
using Namespaces = SharpXMPP.Namespaces;

namespace Cyclops.Xmpp.SharpXmpp.Protocol;

public static class MessageEx
{
    private class XmppMessage : IMessage
    {
        internal readonly XMPPMessage Original;
        public XmppMessage(XMPPMessage original)
        {
            Original = original;
        }

        public Jid? From => Original.From.Map();
        public Jid? To => Original.To.Map();
        public string? Subject => Original.Element(XNamespace.Get(Namespaces.JabberClient) + Elements.Subject)?.Value;
        public string? Body => Original.Text;
        public IError? Error => Original.Element(XNamespace.Get(Namespaces.JabberClient) + Elements.Error)?.WrapAsError();
        public MessageType Type => Original.GetMessageType();
    }

    public static IMessage Wrap(this XMPPMessage message) => new XmppMessage(message);
    public static XMPPMessage Unwrap(this IMessage message) => ((XmppMessage)message).Original;

    private static MessageType GetMessageType(this XMPPMessage message)
    {
        var typeName = message.Attribute("type")?.Value;
        return typeName switch
        {
            "chat" => MessageType.Chat,
            "error" => MessageType.Error,
            "groupchat" => MessageType.GroupChat,
            "headline" => MessageType.Headline,
            null or "" or "normal" => MessageType.Normal,
            _ => throw new NotSupportedException($"Unknown message type: {typeName}.")
        };
    }

    public static string Map(this MessageType messageType) => messageType switch
    {
        MessageType.Chat => "chat",
        MessageType.Error => "error",
        MessageType.GroupChat => "groupchat",
        MessageType.Headline => "headline",
        MessageType.Normal => "normal",
        _ => throw new ArgumentException($"Invalid message type: {messageType}.", nameof(messageType))
    };
}
