using Cyclops.Core;
using Xunit.Abstractions;

namespace Cyclops.TestFramework;

public class XUnitLoggerAdapter : ILogger
{
    private readonly ITestOutputHelper _helper;
    public XUnitLoggerAdapter(ITestOutputHelper helper)
    {
        _helper = helper;
    }

    public bool VerboseLogging { get; set; }
    public void LogError(string message, Exception? exception)
    {
        _helper.WriteLine($"[ERROR] ${message}" + (exception == null ? "" : $" ${exception}"));
    }

    public void LogInfo(string message, params object[] args)
    {
        _helper.WriteLine("[INFO] " + string.Format(message, args));
    }

    public void LogVerbose(string message, params object[] args)
    {
        if (VerboseLogging)
            _helper.WriteLine("[VERBOSE] " + string.Format(message, args));
    }
}
