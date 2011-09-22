using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Cyclops.Core;
using Cyclops.MainApplication.ViewModel;

namespace Cyclops.MainApplication.View.Dialogs
{
    /// <summary>
    /// Interaction logic for NickAndStatusDialog.xaml
    /// </summary>
    public partial class NickAndStatusDialog : Window
    {
        public NickAndStatusDialog(IConference conference)
        {
            DataContext = new NickAndStatusViewModel(conference, () => DialogResult = true);
            InitializeComponent();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, nickTextBox);
        }
    }
}
