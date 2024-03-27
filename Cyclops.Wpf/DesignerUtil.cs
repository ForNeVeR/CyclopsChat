using System.ComponentModel;
using System.Windows;

namespace Cyclops.Wpf;

public static class DesignerUtil
{
    private static readonly DependencyObject Getter = new();
    public static bool IsInDesignMode => DesignerProperties.GetIsInDesignMode(Getter);
}
