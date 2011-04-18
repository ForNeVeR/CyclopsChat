using System;
using System.Windows;
using Cyclops.Core;

namespace Cyclops.MainApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            AppDomain.CurrentDomain.UnhandledException += CurrentDomainUnhandledException;
        }

        private static void CurrentDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            var logger = ChatObjectFactory.GetLogger();
            if (e.ExceptionObject is Exception) //actually it always is of exception type
                logger.LogError("Unhandled exception", e.ExceptionObject as Exception);
        }
    }
}