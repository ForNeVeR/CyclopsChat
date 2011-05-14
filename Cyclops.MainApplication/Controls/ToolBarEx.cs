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
            ToolBar toolBar = sender as ToolBar;
            if (toolBar == null)
                return;
            var overflowGrid = toolBar.Template.FindName("OverflowGrid", toolBar) as FrameworkElement;
            if (overflowGrid != null)
            {
                overflowGrid.Visibility = Visibility.Collapsed;
            }
        }
    }
}
