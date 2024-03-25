using System;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.CustomEventArgs
{
    public class BannedEventArgs : EventArgs
    {
        public BannedEventArgs(Jid? author, string reason)
        {
            Author = author;
            Reason = reason;
        }

        public Jid? Author { get; private set; }
        public string Reason { get; private set; }
    }
}
