using System;
using System.Windows;
using Cyclops.MainApplication.Controls;
using Cyclops.MainApplication.Helpers;
using Cyclops.MainApplication.View;

namespace Cyclops.MainApplication
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow 
    {
        public MainWindow()
        {
            InitializeComponent();
            ThemeManager.ApplyDefault();
        }

        protected override void OnSourceInitialized(EventArgs e)
        {
            GlassEffectHelper.ExtendGlassFrame(this);
            base.OnSourceInitialized(e);
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            TrayController.Instance.Dispose();
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            ViewController.Control(this);
        }
    }
}