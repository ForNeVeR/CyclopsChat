using Cyclops.Core.Configuration;
using GalaSoft.MvvmLight;

namespace Cyclops.MainApplication.Configuration
{
    public class Profile : ViewModelBase
    {
        private ConnectionConfig connectionConfig;
        private string name;

        private string theme;

        public Profile()
        {
            Theme = "Default";
            ConnectionConfig = new ConnectionConfig();
        }

        /// <summary>
        /// Name of the profile
        /// </summary>
        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        /// <summary>
        /// Default theme for current profile. Themes are stored in %APPFOLDER%\Themes folder
        /// </summary>
        public string Theme
        {
            get { return theme; }
            set
            {
                theme = value;
                RaisePropertyChanged("Theme");
            }
        }

        /// <summary>
        /// Assigned configuration
        /// </summary>
        public ConnectionConfig ConnectionConfig
        {
            get { return connectionConfig; }
            set
            {
                connectionConfig = value;
                RaisePropertyChanged("ConnectionConfig");
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}