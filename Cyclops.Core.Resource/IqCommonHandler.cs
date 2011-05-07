using System;
using System.Configuration;
using System.Reflection;
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
                ver.OS = Environment.OSVersion.ToString();
                ver.EntityName = ConfigurationManager.AppSettings["ApplicationName"];
                ver.Ver = Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
            }
            jc.Write(iq);
            return;
        } 
        #endregion
    }
}
