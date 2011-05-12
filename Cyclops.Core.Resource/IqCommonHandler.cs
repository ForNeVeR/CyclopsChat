using System;
using System.Configuration;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using bedrock.util;
using jabber.client;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Cyclops.Core.Resource
{
    public class IqCommonHandler
    {
        public static void Handle(JabberClient jc, IQ iq)
        {
            //process only requests
            if (iq.Type != IQType.get)
                return;

            if (iq.Query == null)
                return;

            HandleTime(jc, iq);
            HandleLast(jc, iq);
            HandleVersion(jc, iq);
        }

        #region Private members
        private static void HandleTime(JabberClient jc, IQ iq)
        {
            var query = iq.Query as jabber.protocol.iq.Time;
            if (query == null)
                return;

            iq = iq.GetResponse(jc.Document);
            Time tim = iq.Query as Time;
            if (tim != null) tim.SetCurrentTime();
            jc.Write(iq);
        }

        private static void HandleLast(JabberClient jc, IQ iq)
        {
            var query = iq.Query as jabber.protocol.iq.Last;
            if (query == null)
                return;
            iq = iq.GetResponse(jc.Document);
            Last last = iq.Query as Last;
            if (last != null) last.Seconds = (int)IdleTime.GetIdleTime();
            jc.Write(iq);
            return;
        }

        private static void HandleVersion(JabberClient jc, IQ iq)
        {
            var query = iq.Query as jabber.protocol.iq.Version;
            if (query == null)
                return;

            iq = iq.GetResponse(jc.Document);
            var ver = iq.Query as jabber.protocol.iq.Version;
            if (ver != null)
            {
                ver.OS = GetOsVersion();
                ver.EntityName = ConfigurationManager.AppSettings["ApplicationName"];
                ver.Ver = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            }
            jc.Write(iq);
            return;
        } 

        #endregion


        #region Get OS version
        private static string GetOsVersion()
        {
            var os = Environment.OSVersion;
            string result = "";
            if (os.Version.Major == 6)
            {
                if (os.Version.Minor == 0)
                    result = "Windows Vista";
                else if (os.Version.Minor == 1)
                    result = "Windows 7";
                else if (os.Version.Minor == 3)
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
