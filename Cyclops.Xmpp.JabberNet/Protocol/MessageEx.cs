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
        public IError? Error => message.Error.Wrap();
    }

    public static IMessage Wrap(this JNMessage message) => new Message(message);
}
