using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;

namespace Cyclops.MainApplication.Helpers
{
    public class GlassEffectHelper
    {
        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern void DwmExtendFrameIntoClientArea(IntPtr hwnd, ref MARGINS margins);

        [DllImport("dwmapi.dll", PreserveSig = false)]
        private static extern bool DwmIsCompositionEnabled();

        public static bool ExtendGlassFrame(Window window)
        {
            return true;
            if (Environment.OSVersion.Version.Major < 6 || !DwmIsCompositionEnabled())
                return false;

            IntPtr hWnd = new WindowInteropHelper(window).Handle;
            window.Background = Brushes.Transparent;
            HwndSource.FromHwnd(hWnd).CompositionTarget.BackgroundColor = Colors.Transparent;
            var margins = new MARGINS(-1, -1, -1, -1);
            DwmExtendFrameIntoClientArea(hWnd, ref margins);
            return true;
        }

        #region Nested type: MARGINS

        [StructLayout(LayoutKind.Sequential)]
        private struct MARGINS
        {
            public MARGINS(int Left, int Right, int Top, int Bottom)
            {
                cxLeftWidth = Left;
                cxRightWidth = Right;
                cyTopHeight = Top;
                cyBottomHeight = Bottom;
            }

            public int cxLeftWidth, cxRightWidth, cyTopHeight, cyBottomHeight;
        }

        #endregion
    }
}