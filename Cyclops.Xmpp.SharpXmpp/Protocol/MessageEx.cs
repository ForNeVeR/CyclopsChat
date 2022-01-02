using System.Xml.Linq;
using Cyclops.Xmpp.Protocol;
using SharpXMPP.XMPP.Client.Elements;

namespace Cyclops.Xmpp.SharpXmpp.Protocol;

public static class MessageEx
{
    private class Message : IMessage
    {
        private readonly XMPPMessage original;
        public Message(XMPPMessage original)
        {
            this.original = original;
        }

        public Jid? From => original.From.Map();
        public Jid? To => original.To.Map();
        public string? Subject => throw new NotImplementedException();
        public string? Body => original.Text;
        public IError? Error => throw new NotImplementedException();
        public MessageType Type => original.GetMessageType();
    }

    public static IMessage Wrap(this XMPPMessage message) => new Message(message);

    private static MessageType GetMessageType(this XMPPMessage message)
    {
        var typeName = message.Attribute(XNamespace.Get(SharpXMPP.Namespaces.JabberClient) + "type")?.Value;
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
}
