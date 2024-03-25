using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.Core.CustomEventArgs
{
    public class NickChangeEventArgs : EventArgs
    {
        public string OldNick { get; private set; }
        public string NewNick { get; private set; }

        public NickChangeEventArgs(string oldNick, string newNick)
        {
            OldNick = oldNick;
            NewNick = newNick;
        }
    }
}
