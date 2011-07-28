using System;
using Cyclops.MainApplication.Options.Model;
using Cyclops.MainApplication.Options.ViewModel;

namespace Cyclops.MainApplication.Options.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView
    {
        private SettingsViewModel viewModel = null;
        private bool okResult = false;
        private Action<ApplicationSettings> commiter = null;

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
                commiter(viewModel.Settings);
            base.OnClosing(e);
        }

        public static void ShowSettings(ApplicationSettings settingsToLoad, Action<ApplicationSettings> commiter)
        {
            SettingsView view = new SettingsView();
            view.viewModel.Settings = settingsToLoad.CreateCopy();
            view.commiter = commiter;
            view.Show();
        }
    }
}
