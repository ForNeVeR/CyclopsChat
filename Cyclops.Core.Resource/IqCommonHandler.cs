using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using Cyclops.Windows;
using Cyclops.Xmpp.Data;
using Cyclops.Xmpp.Protocol.IqQueries;

namespace Cyclops.Core.Resource
{
    public class IqCommonHandler
    {
        public static void HandleTime(IUserSession session, ITimeIq iq)
        {
            var response = iq.CreateResponse();
            response.Time = (DateTime.Now, TimeZoneInfo.Local);

            session.SendIq(iq);
        }

        public static void HandleLast(IUserSession session, ILastIq iq)
        {
            var response = iq.CreateResponse();
            response.Seconds = checked((int)LastInputDetector.GetTimeSinceLastInput().TotalSeconds);

            session.SendIq(response);
        }

        public static void HandleVersion(IUserSession session, IVersionIq iq)
        {
            var response = iq.CreateResponse();
            response.ClientInfo = new ClientInfo(
                GetOsVersion(),
                Assembly.GetExecutingAssembly().GetName().Version.ToString(3),
                ConfigurationManager.AppSettings["ApplicationName"]);

            session.SendIq(response);
        }


        #region Get OS version
        private static string GetOsVersion()
        {
            var os = Environment.OSVersion;
            string result = "";
            if (os.Version.Major == 7 && os.Version.Minor == 0)
            {
                result = "Windows 8";//windows 8 is 6.2 or 7.0 ?
            }
            if (os.Version.Major == 6)
            {
                if (os.Version.Minor == 0)
                    result = "Windows Vista";
                else if (os.Version.Minor == 1)
                    result = "Windows 7";
                else if (os.Version.Minor == 2)
                    result = "Windows 8";
                else if (os.Version.Minor == 3) //sure?
                    result = "Windows Server 2008";
            }
            else if (os.Version.Major == 5)
            {
                if (os.Version.Minor == 0)
                    result = "Windows 2000";
                else if (os.Version.Minor == 1)
                    result = "Windows XP";
                else if (os.Version.Minor == 2)
                    result = "Windows Server 2003";
            }
            if (string.IsNullOrEmpty(result))
                return os.ToString();

            var platform = Is64Bit() ? "64bit" : "32bit";

            return string.Format("{0} {1} {2}", result, platform, os.ServicePack);
        }


        [DllImport("kernel32.dll", SetLastError = true, CallingConvention = CallingConvention.Winapi)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool IsWow64Process([In] IntPtr hProcess, [Out] out bool lpSystemInfo);

        private static bool Is64Bit()
        {
            return IntPtr.Size == 8 || (IntPtr.Size == 4 && Is32BitProcessOn64BitProcessor());
        }

        private static bool Is32BitProcessOn64BitProcessor()
        {
            bool retVal;
            IsWow64Process(Process.GetCurrentProcess().Handle, out retVal);
            return retVal;
        }
        #endregion
    }
}
