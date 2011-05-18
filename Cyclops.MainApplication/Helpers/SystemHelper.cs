using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace Cyclops.MainApplication.Helpers
{
    public static class SystemHelper
    {
        public static void StartAppWithWindows(bool enable)
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
                if (key == null)
                    return;

                if (enable)
                    key.SetValue("CyclopsChat", "\"" + Assembly.GetExecutingAssembly().Location + "\"");
                else if (key.GetValue("CyclopsChat") != null)
                    key.DeleteValue("CyclopsChat");
                key.Close();
            }
            catch
            {
            }
        }
    }
}
