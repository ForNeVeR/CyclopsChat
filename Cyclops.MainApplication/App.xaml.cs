using System;
using System.Windows;
using Cyclops.Core;
using Cyclops.Core.Resource;

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

        void DispatchernhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            var logger = ChatObjectFactory.GetLogger();
            logger.LogError("Unhandled exception", e.Exception);
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var logger = ChatObjectFactory.GetLogger();
            if (e.ExceptionObject is Exception) //actually it always is of exception type
                logger.LogError("Unhandled exception", e.ExceptionObject as Exception);
        }
    }
}