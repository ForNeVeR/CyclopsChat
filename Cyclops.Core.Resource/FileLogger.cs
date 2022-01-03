using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text;

namespace Cyclops.Core.Resource
{
    [SuppressMessage("ReSharper", "LocalizableElement")]
    public class FileLogger : ILogger
    {
        private readonly object errorLocker = new();
        private readonly object infoLocker = new();
        private readonly object verboseLocker = new();

        private volatile bool verboseLogging;
        public bool VerboseLogging
        {
            get => verboseLogging;
            set => verboseLogging = value;
        }

        public void LogError(string message, Exception? exception)
        {
            lock (errorLocker)
            {
                StringBuilder error = new StringBuilder();
                error.Append($"\n============\n{DateTime.Now}\n============\n{message}\n");
                if (exception != null)
                    error.Append($"{exception.Message}\n{exception.StackTrace}\n");
                error.Append('\n');
                File.AppendAllText("errors.log", error.ToString());
            }

            if (exception?.InnerException != null)
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

        public void LogVerbose(string message, params object[] args)
        {
            if (!verboseLogging) return;
            lock (verboseLocker)
                File.AppendAllText("verbose.log", $"[{DateTime.Now:s}] {string.Format(message, args)}\n");
        }
    }
}
