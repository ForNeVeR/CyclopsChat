using System;

namespace Cyclops.Core
{
    public interface ILogger
    {
        public bool VerboseLogging { set; }

        void LogError(string message, Exception? exception);
        void LogInfo(string message, params object[] args);
        void LogVerbose(string message, params object[] args);
    }
}
