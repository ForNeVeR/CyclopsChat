using System;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.CustomEventArgs
{
    public class KickedEventArgs : EventArgs
    {
        public KickedEventArgs(IEntityIdentifier author, string? reason)
        {
            Author = author;
            Reason = reason;
        }

        public IEntityIdentifier Author { get; }
        public string? Reason { get; }
    }
}
