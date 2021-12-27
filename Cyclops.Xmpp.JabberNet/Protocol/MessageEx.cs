using Cyclops.Xmpp.Protocol;
using jabber.protocol;
using JNMessage = jabber.protocol.client.Message;

namespace Cyclops.Xmpp.JabberNet.Protocol;

public static class MessageEx
{
    private class Message : Stanza, IMessage
    {
        public Message(Packet message) : base(message)
        {
        }
    }

    public static IMessage Wrap(this JNMessage message) => new Message(message);
}
