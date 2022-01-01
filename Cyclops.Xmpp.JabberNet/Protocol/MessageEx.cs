using Cyclops.Xmpp.Protocol;
using JNMessage = jabber.protocol.client.Message;

namespace Cyclops.Xmpp.JabberNet.Protocol;

public static class MessageEx
{
    private class Message : Stanza, IMessage
    {
        private readonly JNMessage message;
        public Message(JNMessage message) : base(message)
        {
            this.message = message;
        }

        public string? Subject => message.Subject;
        public string? Body => message.Body;
        public IError? Error => message.Error?.Wrap();
        public MessageType Type => message.Type.Map();
    }

    public static IMessage Wrap(this JNMessage message) => new Message(message);

    private static MessageType Map(this jabber.protocol.client.MessageType type) => type switch
    {
        jabber.protocol.client.MessageType.normal => MessageType.Normal,
        jabber.protocol.client.MessageType.error => MessageType.Error,
        jabber.protocol.client.MessageType.chat => MessageType.Chat,
        jabber.protocol.client.MessageType.groupchat => MessageType.GroupChat,
        jabber.protocol.client.MessageType.headline => MessageType.Headline,
        _ => throw new ArgumentException($"Unknown message type: {type}.", nameof(type))
    };

    public static jabber.protocol.client.MessageType Map(this MessageType type) => type switch
    {
        MessageType.Normal => jabber.protocol.client.MessageType.normal,
        MessageType.Error => jabber.protocol.client.MessageType.error,
        MessageType.Chat => jabber.protocol.client.MessageType.chat,
        MessageType.GroupChat => jabber.protocol.client.MessageType.groupchat,
        MessageType.Headline => jabber.protocol.client.MessageType.headline,
        _ => throw new ArgumentException($"Unknown message type: {type}.", nameof(type))
    };
}
