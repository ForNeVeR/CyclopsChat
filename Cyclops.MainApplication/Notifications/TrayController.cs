using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Threading;
using Cyclops.MainApplication.Properties;
using Application = System.Windows.Application;

namespace Cyclops.MainApplication.Notifications
{
    public class TrayController : IDisposable
    {
        private static TrayController instance;
        private readonly Icon defaultIcon = Resources.ligth_on;
        private readonly Icon emptyIcon = Resources.EmptyIcon;
        private readonly NotifyIcon notifyIcon = new NotifyIcon();
        private readonly DispatcherTimer timer = new DispatcherTimer();
        private readonly Window window;

        private TrayController(Window window)
        {
            this.window = window;
            timer.Interval = TimeSpan.FromMilliseconds(300);
            timer.Tick += TimerTick;

            notifyIcon.BalloonTipTitle = "CyclopsChat";
            notifyIcon.Text = notifyIcon.BalloonTipText = "Cyclops chat (double-click to show/hide)";
            notifyIcon.Visible = false;
            notifyIcon.MouseDown += NotifyIconMouseDown;
            notifyIcon.MouseDoubleClick += NotifyIconMouseDoubleClick;
            if (window != null)
                window.StateChanged += WindowStateChanged;
        }

        void NotifyIconMouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var menu = (System.Windows.Controls.ContextMenu)window.FindResource("trayMenu");
                menu.IsOpen = true;
            }
        }

        void WindowStateChanged(object sender, EventArgs e)
        {
            if (window.WindowState == WindowState.Minimized &&
                !ApplicationContext.Current.Settings.HideOnWindowClosing)
                window.Hide();
        }


        public static TrayController Instance
        {
            get
            {
                if (instance == null)
                    instance = new TrayController(Application.Current.MainWindow);
                return instance;
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            notifyIcon.Icon = null;
            notifyIcon.Dispose();
        }

        #endregion

        private void NotifyIconMouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                HideOrShowWindow(window);
            }
        }

        public static void HideOrShowWindow(Window window)
        {
            if (window.IsVisible)
                window.Hide();
            else
            {
                window.Show();
                window.WindowState = WindowState.Normal;
                //workaround:)
                window.Activate();
                window.Topmost = true;  // important
                window.Topmost = false; // important
                window.Focus();
            }
        }

        private void TimerTick(object sender, EventArgs e)
        {
            if (notifyIcon.Icon == emptyIcon)
                notifyIcon.Icon = defaultIcon;
            else
                notifyIcon.Icon = emptyIcon;
        }

        public void StartBlink()
        {
            if (ApplicationContext.Current.Settings.DisableBlinking)
                return;
            
            timer.Start();
        }

        public void StopBlink()
        {
            if (!timer.IsEnabled)
                return;

            timer.Stop();
            notifyIcon.Icon = defaultIcon;
        }

        public void ShowDefaultIcon()
        {
            notifyIcon.Visible = true;
            notifyIcon.Icon = Resources.ligth_on;
        }
    }
}