using System;

namespace Cyclops.Core
{
    public interface ILogger
    {
        void LogError(string message, Exception? exception);
        void LogInfo(string message, params object[] args);
    }
}
