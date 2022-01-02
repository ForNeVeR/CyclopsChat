using System;
using System.Windows.Threading;

namespace Cyclops.Core.Resource.Helpers;

public static class DispatcherEx
{
    public static void InvokeAsyncIfRequired(this Dispatcher dispatcher, Action action)
    {
        if (dispatcher.CheckAccess())
            action();
        else
            dispatcher.InvokeAsync(action);
    }
}
