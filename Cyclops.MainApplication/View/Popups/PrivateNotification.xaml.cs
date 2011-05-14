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
using GalaSoft.MvvmLight;

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
