using System;
using System.Windows;
using Cyclops.MainApplication.Options.Model;
using Cyclops.MainApplication.Options.ViewModel;

namespace Cyclops.MainApplication.Options.View;

/// <summary>
/// Interaction logic for SettingsView.xaml
/// </summary>
public partial class SettingsView
{
    private readonly SettingsViewModel viewModel;
    private bool okResult;
    private Action<ApplicationSettings>? committer;

    protected SettingsView()
    {
        DataContext = viewModel = new SettingsViewModel(Ok, Cancel);
        InitializeComponent();
    }

    private void Cancel()
    {
        okResult = false;
        Close();
    }

    private void Ok()
    {
        okResult = true;
        Close();
    }

    protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
    {
        if (okResult)
            committer!(viewModel.Settings!);
        base.OnClosing(e);
    }

    public static void ShowSettings(ApplicationSettings settingsToLoad, Action<ApplicationSettings> commiter)
    {
        var view = new SettingsView
        {
            Owner = Application.Current.MainWindow,
            viewModel =
            {
                Settings = settingsToLoad.CreateCopy()
            },
            committer = commiter
        };
        view.Show();
    }
}
