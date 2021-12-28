using System;
using System.Collections.Generic;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.CustomEventArgs
{
    public class ConferencesListEventArgs : EventArgs
    {
        public ConferencesListEventArgs()
        {
            Success = false;
        }

        public ConferencesListEventArgs(List<Tuple<Jid, string>> result)
        {
            Result = result;
            Success = true;
        }

        public List<Tuple<Jid, string>> Result { get; private set; }
        public bool Success { get; private set; }
    }
}
