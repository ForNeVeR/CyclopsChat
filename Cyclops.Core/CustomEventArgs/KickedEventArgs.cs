using System;
using Cyclops.Xmpp.Data;

namespace Cyclops.Core.CustomEventArgs
{
    public class KickedEventArgs : EventArgs
    {
        public KickedEventArgs(IEntityIdentifier author, string reason)
        {
            Author = author;
            Reason = reason;
        }

        public IEntityIdentifier Author { get; private set; }
        public string Reason { get; private set; }
    }
}