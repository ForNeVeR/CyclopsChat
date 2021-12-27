using System;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.CustomEventArgs
{
    public class BannedEventArgs : EventArgs
    {
        public BannedEventArgs(IEntityIdentifier author, string reason)
        {
            Author = author;
            Reason = reason;
        }

        public IEntityIdentifier Author { get; private set; }
        public string Reason { get; private set; }
    }
}