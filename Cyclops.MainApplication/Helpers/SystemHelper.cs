using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace Cyclops.MainApplication.Helpers
{
    public static class SystemHelper
    {
        /// <summary>
        /// Add/Remove registry entries for windows startup.
        /// </summary>
        /// <param name="appName">Name of the application.</param>
        /// <param name="enable">if set to <c>true</c> [enable].</param>
        public static void SetStartup(bool enable, string appName = "CyclopsChat")
        {
            try
            {
                const string runKey = @"Software\Microsoft\Windows\CurrentVersion\Run";

                RegistryKey startupKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(runKey);

                if (enable)
                {
                    if (startupKey.GetValue(appName) == null)
                    {
                        startupKey.Close();
                        startupKey = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(runKey, true);
                        // Add startup reg key
                        startupKey.SetValue(appName, Assembly.GetExecutingAssembly().Location, RegistryValueKind.String);
                        startupKey.Close();
                    }
                }
                else
                {
                    // remove startup
                    startupKey = Registry.CurrentUser.OpenSubKey(runKey, true);
                    startupKey.DeleteValue(appName, false);
                    startupKey.Close();
                }
            }
            catch (SecurityException)
            {
                //TODO: LOG it!
            }
            catch (Exception)
            {
            }
        }
    }
}
