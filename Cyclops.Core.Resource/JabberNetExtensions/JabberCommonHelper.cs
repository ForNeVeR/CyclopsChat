using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jabber;
using jabber.connection;
using jabber.protocol.client;
using jabber.protocol.iq;

namespace Cyclops.Core.Resource.JabberNetExtensions
{
    public static class JabberCommonHelper
    {
        public static void GetClientVersionAsnyc(UserSession session, JID nickJid, IqCB callback)
        {
            var versionIq = new VersionIQ(session.JabberClient.Document) {To = nickJid, Type = IQType.get};
            session.ConferenceManager.BeginIQ(versionIq, callback, null);
        }
    }
}
