using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;
using Microsoft.Win32;

namespace Cyclops.MainApplication.Options.Controls;

/// <summary>
/// Interaction logic for Selector.xaml
/// </summary>
public partial class Selector
{
    public static readonly DependencyProperty TextProperty =
        DependencyProperty.Register("Text", typeof (string), typeof (Selector), new UIPropertyMetadata(""));

    public static readonly DependencyProperty CommandProperty =
        DependencyProperty.Register("Command", typeof (ICommand), typeof (Selector), new UIPropertyMetadata(null));

    public static readonly DependencyProperty IsReadOnlyProperty =
        DependencyProperty.Register("IsReadOnly", typeof (bool), typeof (Selector), new UIPropertyMetadata(false));

    public static readonly DependencyProperty FileSelectionModeProperty =
        DependencyProperty.Register("FileSelectionMode", typeof (bool), typeof (Selector),
            new UIPropertyMetadata(false, FileSelectionModePropertyChanged));

    public static readonly DependencyProperty FileSelectionFilterProperty =
        DependencyProperty.Register("FileSelectionFilter", typeof (string), typeof (Selector),
            new UIPropertyMetadata(""));

    public Selector()
    {
        Clear = new RelayCommand(() => Text = string.Empty, () => !string.IsNullOrEmpty(Text));
        InitializeComponent();
    }

    public RelayCommand Clear { get; set; }

    public string Text
    {
        get { return (string) GetValue(TextProperty); }
        set { SetValue(TextProperty, value); }
    }

    public ICommand Command
    {
        get { return (ICommand) GetValue(CommandProperty); }
        set { SetValue(CommandProperty, value); }
    }

    public bool IsReadOnly
    {
        get { return (bool) GetValue(IsReadOnlyProperty); }
        set { SetValue(IsReadOnlyProperty, value); }
    }

    public bool FileSelectionMode
    {
        get { return (bool) GetValue(FileSelectionModeProperty); }
        set { SetValue(FileSelectionModeProperty, value); }
    }

    public string FileSelectionFilter
    {
        get { return (string) GetValue(FileSelectionFilterProperty); }
        set { SetValue(FileSelectionFilterProperty, value); }
    }

    private static void FileSelectionModePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is Selector selector && (bool)e.NewValue)
            selector.SetFileSelectionCommand();
    }

    private void SetFileSelectionCommand()
    {
        Command = new RelayCommand(FileSelectAction);
    }

    private void FileSelectAction()
    {
        var dlg = new OpenFileDialog();
        dlg.Filter = FileSelectionFilter;
        dlg.InitialDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        if (dlg.ShowDialog() == true)
            Text = dlg.FileName;
    }
}
