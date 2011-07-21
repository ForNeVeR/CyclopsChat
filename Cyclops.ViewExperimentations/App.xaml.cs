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
            string a = "3r3r";
            bool isc = a is IEnumerable<char>;
        }
    }
}
