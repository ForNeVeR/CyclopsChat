using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Cyclops.MainApplication.Controls
{
    public class ToolBarEx : ToolBar
    {
        public ToolBarEx()
        {
            Loaded += ToolbarExLoaded;
        }

        //removing overflow 
        private void ToolbarExLoaded(object sender, RoutedEventArgs e)
        {
            if (sender is not ToolBar toolBar)
                return;
            if (toolBar.Template.FindName("OverflowGrid", toolBar) is FrameworkElement overflowGrid)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}
