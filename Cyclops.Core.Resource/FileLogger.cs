using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Cyclops.Core.Resource
{
    [SuppressMessage("ReSharper", "LocalizableElement")]
    public class FileLogger : ILogger
    {
        private readonly object errorLocker = new();
        private readonly object infoLocker = new();

        public void LogError(string message, Exception exception)
        {
            lock (errorLocker)
            {
                File.AppendAllText(
                    "errors.log",
                    $"\n============\n{DateTime.Now}\n============\n{message}\n{exception.Message}\n{exception.StackTrace}\n\n");
            }

            if (exception.InnerException != null)
                LogError("InnerException", exception.InnerException);
        }

        public void LogInfo(string message, params object[] args)
        {
            lock (infoLocker)
            {
                File.AppendAllText(
                    "info.log",
                    $"\n============\n{DateTime.Now}\n============\n{string.Format(message, args)}\n\n");
            }
        }
    }
}
