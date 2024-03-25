using System;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.CustomEventArgs
{
    public class ErrorEventArgs : EventArgs
    {
        public ErrorEventArgs(Jid? from, string msg)
        {
            From = from;
            Message = msg;
        }

        public Jid? From { get; private set; }
        public string Message { get; private set; }
    }
}
