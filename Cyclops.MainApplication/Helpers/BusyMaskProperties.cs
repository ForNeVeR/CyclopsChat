using System.Windows;
using BusyIndicator;

namespace Cyclops.MainApplication.Helpers;

public static class BusyMaskProperties
{
    /// <summary>
    /// The same as <see cref="BusyMask.IsBusy"/> but with <see cref="BusyMask.IsBusyAtStartup"/> combined, so it works
    /// properly for the cases when <see cref="BusyMask.IsBusy"/> was <c>true</c> at the moment of the control creation.
    /// </summary>
    /// <remarks>
    /// See <a href="https://github.com/Moh3nGolshani/BusyIndicator/issues/24">BusyIndicator#24</a> for more details.
    /// </remarks>
    public static readonly DependencyProperty IsBusyWorkProperty =
        DependencyProperty.RegisterAttached(
            "IsBusyWork",
            typeof(bool),
            typeof(BusyMaskProperties),
            new PropertyMetadata(false, OnIsBusyWorkChanged));

    public static bool GetIsBusyWork(DependencyObject obj) => (bool)obj.GetValue(IsBusyWorkProperty);

    public static void SetIsBusyWork(DependencyObject obj, bool value) => obj.SetValue(IsBusyWorkProperty, value);

    private static void OnIsBusyWorkChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var busyMask = (BusyMask)d;
        busyMask.IsBusyAtStartup = (bool)e.NewValue;
        busyMask.IsBusy = (bool)e.NewValue;
    }
}
