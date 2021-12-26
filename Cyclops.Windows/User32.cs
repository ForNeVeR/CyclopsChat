using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;

namespace Cyclops.Windows;

// ReSharper disable InconsistentNaming
[SuppressMessage("ReSharper", "UnassignedField.Global")]
public static class User32
{
    [DllImport("User32")]
    public static extern bool GetLastInputInfo(ref LASTINPUTINFO plii);

    public struct LASTINPUTINFO
    {
        public uint cbSize;
        public uint dwTime;
    }
}
