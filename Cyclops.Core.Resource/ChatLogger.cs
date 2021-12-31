using System.Configuration;
using System.IO;
using Cyclops.Xmpp.Protocol;

namespace Cyclops.Core.Resource
{
    public class ChatLogger : IChatLogger
    {
        private readonly string chatDirectory = string.Empty;

        public ChatLogger()
        {
            chatDirectory = ConfigurationManager.AppSettings["ChatLogsDirectory"];
            if (!Directory.Exists(chatDirectory))
                Directory.CreateDirectory(chatDirectory);
        }

        private static readonly object SyncObject = new object();

        #region Implementation of IChatLogManager

        public void AddRecord(Jid? id, string message, bool isPrivate = false)
        {
            lock (SyncObject)
            {
                if (id == null)
                    return;
                try
                {
                    string file = BuildPath(id.Value, isPrivate);
                    using (var ws = File.AppendText(file))
                        ws.WriteLine(message ?? string.Empty);
                }
                    // ReSharper disable EmptyGeneralCatchClause
                catch
                    // ReSharper restore EmptyGeneralCatchClause
                {
                }
            }
        }

        private string BuildPath(Jid id, bool isPrivate)
        {
            string jid = string.Format("{0}@{1}", id.Local, id.Domain);
            foreach (var c in Path.GetInvalidFileNameChars())
                jid = jid.Replace(c, '_');

            if (isPrivate)
            {
                var dir = Path.Combine(chatDirectory, "Privates");
                if (!Directory.Exists(dir))
                    Directory.CreateDirectory(dir);
                return Path.Combine(dir, jid);
            }

            return Path.Combine(chatDirectory, jid);
        }

        #endregion
    }
}
