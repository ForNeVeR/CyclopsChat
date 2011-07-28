using Cyclops.MainApplication.Options.ViewModel;

namespace Cyclops.MainApplication.Options.View
{
    /// <summary>
    /// Interaction logic for SettingsView.xaml
    /// </summary>
    public partial class SettingsView 
    {
        public SettingsView()
        {
            DataContext = new SettingsViewModel(() => { }, () => { });
            InitializeComponent();
        }

        public static void ShowSettings()
        {
        }
    }
}
