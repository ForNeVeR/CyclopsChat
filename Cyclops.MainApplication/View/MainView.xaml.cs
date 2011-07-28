using System;
using System.Configuration;
using System.Windows.Controls;
using Cyclops.MainApplication.Options.View;
using Cyclops.MainApplication.ViewModel;

namespace Cyclops.MainApplication.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : IMainView
    {
        public MainView()
        {
            InitializeComponent();

            DataContext = new MainViewModel(this);

            var stylingMode = ConfigurationManager.AppSettings["StylingMode"];
            if (stylingMode != null && stylingMode.ToLower().Equals("true"))
                refreshButton.Visibility = System.Windows.Visibility.Visible;
            else
                refreshButton.Visibility = System.Windows.Visibility.Hidden;
        }

        #region Implementation of IMainView

        public void ShowSettings()
        {
            SettingsView view = new SettingsView();
            view.Show();
        }

        #endregion
    }
}