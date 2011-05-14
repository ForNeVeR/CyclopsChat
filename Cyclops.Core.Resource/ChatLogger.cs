using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;

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

        #region Implementation of IChatLogManager

        public void AddRecord(IEntityIdentifier id, string message, bool isPrivate = false)
        {
            if (id == null)
                return;
            try
            {
                string file = BuildPath(id);
                using (var ws = File.AppendText(file))
                    ws.WriteLine(message ?? string.Empty);
            }
// ReSharper disable EmptyGeneralCatchClause
            catch
// ReSharper restore EmptyGeneralCatchClause
            {
            }
        }

        private string BuildPath(IEntityIdentifier id)
        {
            string jid = string.Format("{0}@{1}", id.User, id.Server);
            foreach (var c in Path.GetInvalidFileNameChars())
                jid = jid.Replace(c, ' ');

            return Path.Combine(chatDirectory, jid);
        }

        #endregion
    }
}
