using System.Threading.Tasks;

namespace Cyclops.Core.Helpers;

public static class TaskEx
{
    public static void NoAwait(this Task task, ILogger logger)
    {
        task.ContinueWith(t =>
        {
            if (t.IsFaulted)
                logger.LogError("Task faulted with an exception", t.Exception!);
        });
    }
}
