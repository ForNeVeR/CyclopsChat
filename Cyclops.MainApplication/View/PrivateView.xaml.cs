using System.Windows;
using System.Windows.Controls;
using Cyclops.MainApplication.ViewModel;

namespace Cyclops.MainApplication.View
{
    /// <summary>
    /// Interaction logic for PrivateView.xaml
    /// </summary>
    public partial class PrivateView : UserControl
    {
        public static readonly DependencyProperty PrivateViewModelProperty =
            DependencyProperty.Register("PrivateViewModel", typeof (PrivateViewModel), typeof (PrivateView),
                                        new UIPropertyMetadata(null));

        public PrivateView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public PrivateViewModel PrivateViewModel
        {
            get { return (PrivateViewModel) GetValue(PrivateViewModelProperty); }
            set { SetValue(PrivateViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for PrivateViewModel.  This enables animation, styling, binding, etc...
    }
}