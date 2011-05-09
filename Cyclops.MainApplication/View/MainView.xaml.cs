using System.Configuration;
using System.Windows.Controls;

namespace Cyclops.MainApplication.View
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        public MainView()
        {
            InitializeComponent();
            var stylingMode = ConfigurationManager.AppSettings["StylingMode"];
            if (stylingMode != null && stylingMode.ToLower().Equals("true"))
                refreshButton.Visibility = System.Windows.Visibility.Visible;
            else
                refreshButton.Visibility = System.Windows.Visibility.Hidden;
        }
    }
}