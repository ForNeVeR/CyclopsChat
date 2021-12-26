using System.Diagnostics;
using static Cyclops.Windows.User32;

namespace Cyclops.Windows;

public class LastInputDetector : IDisposable
{
    private readonly Timer timer;
    private readonly TimeSpan idleInterval;
    private bool? currentlyIdle;
    private volatile bool disabled;

    public event EventHandler<TimeSpan>? IdleModePeriodic;
    public event EventHandler? LeaveIdleMode;

    public LastInputDetector(TimeSpan pollInterval, TimeSpan idleInterval)
    {
        this.idleInterval = idleInterval;

        timer = new Timer(TimerCallback, null, TimeSpan.Zero, pollInterval);
    }

    private unsafe void TimerCallback(object _)
    {
        var lastInputInfo = new LASTINPUTINFO { cbSize = (uint)sizeof(LASTINPUTINFO) };
        if (!GetLastInputInfo(ref lastInputInfo)) return;
        var currentTime = Environment.TickCount;

        var inputIdle = TimeSpan.FromMilliseconds(currentTime - lastInputInfo.dwTime);
        Debug.WriteLine(inputIdle);
        if (inputIdle >= idleInterval)
            NotifyInIdleMode(inputIdle);
        else
            NotifyOutOfIdleMode();
    }

    public void Dispose()
    {
        disabled = true;
        timer.Dispose();
    }

    private void NotifyInIdleMode(TimeSpan idleTime)
    {
        if (disabled) return;

        currentlyIdle = true;
        IdleModePeriodic?.Invoke(this, idleTime);
    }

    private void NotifyOutOfIdleMode()
    {
        if (disabled || currentlyIdle == false) return;

        currentlyIdle = false;
        LeaveIdleMode?.Invoke(this, null);
    }
}
