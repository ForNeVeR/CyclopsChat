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

        public ConferencesListEventArgs(List<Tuple<IEntityIdentifier, string>> result)
        {
            Result = result;
            Success = true;
        }

        public List<Tuple<IEntityIdentifier, string>> Result { get; private set; }
        public bool Success { get; private set; }
    }
}
