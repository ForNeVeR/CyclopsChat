using System;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.MainApplication.MessageDecoration.Decorators
{
    public class NickEventArgs : EventArgs
    {
        public Jid Id { get; private set; }
        public string Nick { get; private set; }

        public NickEventArgs(Jid nickId, string nick)
        {
            Nick = nick;
            Id = nickId;
        }
    }
}
