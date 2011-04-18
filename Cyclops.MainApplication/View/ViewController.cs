using System;
using System.Windows.Controls;
using System.Windows.Input;
using Cyclops.MainApplication.ViewModel;

namespace Cyclops.MainApplication.View
{
    /// <summary>
    /// TODO: move code to MainViewModel
    /// </summary>
    public static class ViewController
    {
        private static LoginView loginView;
        private static MainWindow mainWindow;
        private static MainView mainView;

        public static void Control(MainWindow window)
        {
            mainWindow = window;

            loginView = new LoginView();
            ((LoginViewModel) loginView.DataContext).Authenticated += ViewControllerLoginAuthenticated;
            SetActiveView(loginView);
        }

        private static void ViewControllerLoginAuthenticated(object sender, EventArgs e)
        {
            ((IDisposable) loginView.DataContext).Dispose();
            SetActiveView(mainView = new MainView());
        }

        private static void SetActiveView(UserControl control)
        {
            mainWindow.Content = control;
            FocusManager.SetFocusedElement(mainWindow, control);
        }
    }
}