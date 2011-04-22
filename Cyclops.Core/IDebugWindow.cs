using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.Core
{
    public interface IDebugWindow
    {
        void ShowWindow(IUserSession session);
    }
}
