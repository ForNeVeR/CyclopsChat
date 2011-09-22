using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Cyclops.MainApplication.Helpers
{
    public static class MemoryUtility
    {
        [DllImport("psapi.dll")]
        public static extern bool EmptyWorkingSet(IntPtr hProcess);

        public static void Clean()
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            EmptyWorkingSet(Process.GetCurrentProcess().Handle);
        }
    }
}
