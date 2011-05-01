using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.MainApplication.MessageDecoration.Decorators
{
    public class StringEventArgs : EventArgs
    {
        public string String { get; private set; }

        public StringEventArgs(string value)
        {
            String = value;
        }
    }
}
