using System;

namespace Cyclops.Core.CustomEventArgs
{
    public class RoleChangedEventArgs : EventArgs
    {
        public string By { get; set; }
        public string? To { get; set; }
        public Role Role { get; set; }
    }
}
