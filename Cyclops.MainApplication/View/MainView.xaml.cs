using System.Configuration;
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

            DataContext = new MainViewModel(ChatObjectFactory.GetDebugLogger(), this);

            var stylingMode = ConfigurationManager.AppSettings["StylingMode"];
            if (stylingMode != null && stylingMode.ToLower().Equals("true"))
                refreshButton.Visibility = System.Windows.Visibility.Visible;
            else
                refreshButton.Visibility = System.Windows.Visibility.Hidden;
        }

        #region Implementation of IMainView

        public void ShowSettings()
        {
            ApplicationContext.Current.ReloadApplicationSettings();
            SettingsView.ShowSettings(ApplicationContext.Current.ApplicationSettings, s =>
                {
                    ApplicationContext.Current.ApplicationSettings = s.CreateCopy();
                    ApplicationContext.Current.ApplicationSettings.Save();
                    ApplicationContext.Current.ReloadApplicationSettings();
                });
        }

        #endregion
    }
}
