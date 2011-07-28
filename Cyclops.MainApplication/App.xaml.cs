using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using Cyclops.Core;
using Cyclops.Core.Resource;
using Cyclops.MainApplication.Configuration;
using Cyclops.MainApplication.Localization;
using Cyclops.MainApplication.View.Popups;

namespace Cyclops.MainApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            Dispatcher.UnhandledException += DispatchernhandledException;
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            EventManager.RegisterClassHandler(typeof(Window), Window.LoadedEvent, new RoutedEventHandler(WindowLoaded));
            base.OnStartup(e);
        }

        static void DispatchernhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var logger = ChatObjectFactory.GetDebugLogger();
            logger.LogError("Unhandled exception", e.Exception);
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var logger = ChatObjectFactory.GetDebugLogger();
            if (e.ExceptionObject is Exception) //actually it always is of exception type
                logger.LogError("Unhandled exception", e.ExceptionObject as Exception);
        }

        static void WindowLoaded(object sender, RoutedEventArgs e)
        {
            //var window = e.Source as Window;
            //System.Threading.Thread.Sleep(100);
            //window.Dispatcher.Invoke(
            //new Action(() => window.MoveFocus(new TraversalRequest(FocusNavigationDirection.First))));
        }
    }
}