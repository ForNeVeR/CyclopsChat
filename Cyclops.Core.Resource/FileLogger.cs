using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Cyclops.Core.Resource
{
    public class FileLogger : ILogger
    {
        public void LogError(string message, Exception exception)
        {
            File.AppendAllText("errors.log", string.Format("============\n{0}============\n{1}\n{2}\n{3}\n\n", 
                DateTime.Now, message, exception.Message, exception.StackTrace));
        }

        public void LogInfo(string message, params object[] args)
        {
            File.AppendAllText("info.log", string.Format("============\n{0}============\n{1}\n\n",
                DateTime.Now, string.Format(message, args)));
        }
    }
}
