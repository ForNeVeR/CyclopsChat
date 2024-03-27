using System.Windows.Input;

namespace Cyclops.MainApplication.View.Popups
{
    /// <summary>
    /// Interaction logic for PrivateNotification.xaml
    /// </summary>
    public partial class PrivateNotification
    {
        public PrivateNotification()
        {
            InitializeComponent();
        }

        protected override void OnPreviewMouseDown(MouseButtonEventArgs e)
        {
            if (e.RightButton == MouseButtonState.Pressed)
            {
                e.Handled = true;
                Close();
            }
            else if (e.LeftButton == MouseButtonState.Pressed)
            {
                ((PrivateNotificationViewModel)DataContext).OnRequestActivate();
                e.Handled = true;
                Close();
            }
            base.OnMouseDown(e);
        }
    }
}
