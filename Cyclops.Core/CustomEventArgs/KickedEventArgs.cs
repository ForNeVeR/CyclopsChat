using System;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.CustomEventArgs
{
    public class KickedEventArgs : EventArgs
    {
        public KickedEventArgs(Jid? author, string? reason)
        {
            Author = author;
            Reason = reason;
        }

        public Jid? Author { get; }
        public string? Reason { get; }
    }
}
