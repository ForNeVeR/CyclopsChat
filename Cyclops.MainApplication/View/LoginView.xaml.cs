using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cyclops.MainApplication.View
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : UserControl
    {
        public LoginView()
        {
            InitializeComponent();
        }

        private void UserControlLoaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, userName);
        }
    }
}