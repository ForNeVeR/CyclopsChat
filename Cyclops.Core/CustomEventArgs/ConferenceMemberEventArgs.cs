using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.Core.CustomEventArgs
{
    public class ConferenceMemberEventArgs : EventArgs
    {
        public ConferenceMemberEventArgs(IConferenceMember member)
        {
            Member = member;
        }

        public IConferenceMember Member { get; private set; }
    }
}
