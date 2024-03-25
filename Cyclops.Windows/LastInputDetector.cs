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

    public static unsafe TimeSpan GetTimeSinceLastInput()
    {
        var lastInputInfo = new LASTINPUTINFO { cbSize = (uint)sizeof(LASTINPUTINFO) };
        if (!GetLastInputInfo(ref lastInputInfo)) return TimeSpan.Zero;

        var currentTime = Environment.TickCount;
        return TimeSpan.FromMilliseconds(currentTime - lastInputInfo.dwTime);
    }

    private void TimerCallback(object? _)
    {
        var inputIdle = GetTimeSinceLastInput();
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
        LeaveIdleMode?.Invoke(this, EventArgs.Empty);
    }
}
