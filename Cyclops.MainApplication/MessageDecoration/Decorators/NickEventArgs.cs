using System;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.MainApplication.MessageDecoration.Decorators
{
    public class NickEventArgs : EventArgs
    {
        public IEntityIdentifier Id { get; private set; }
        public string Nick { get; private set; }

        public NickEventArgs(IEntityIdentifier nickId, string nick)
        {
            Nick = nick;
            Id = nickId;
        }
    }
}
