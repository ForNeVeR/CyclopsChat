using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops.Core;

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
