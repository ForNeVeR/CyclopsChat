using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;

namespace Cyclops.ViewExperimentations
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            double a = 1;
            double result = a / 0;

            a = 0;
            result = a / 0;
        }
    }
}
