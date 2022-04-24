using System.Windows;
using System.Windows.Controls;

namespace Cyclops.MainApplication.Options.Controls;

/// <summary>
/// Interaction logic for NamedSeparator.xaml
/// </summary>
public partial class NamedSeparator : UserControl
{
    public NamedSeparator()
    {
        DataContext = this;
        InitializeComponent();
    }

    public string Text
    {
        get { return (string)GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof(string), typeof(NamedSeparator), new UIPropertyMetadata("Common"));
}