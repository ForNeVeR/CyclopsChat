using Cyclops.Xmpp.Protocol;
using JNMessage = jabber.protocol.client.Message;

namespace Cyclops.Xmpp.JabberNet.Protocol;

public static class MessageEx
{
    private class Message : Stanza, IMessage
    {
        public readonly JNMessage OriginalMessage;
        public Message(JNMessage message) : base(message)
        {
            OriginalMessage = message;
        }

        public string? Subject => OriginalMessage.Subject;
        public string? Body => OriginalMessage.Body;
        public IError? Error => OriginalMessage.Error?.Wrap();
        public MessageType Type => OriginalMessage.Type.Map();
    }

    public static IMessage Wrap(this JNMessage message) => new Message(message);
    public static JNMessage Unwrap(this IMessage message) => ((Message)message).OriginalMessage;

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
