using System.Collections.Generic;
using Cyclops.Configuration;
using Cyclops.MainApplication.ViewModel;

namespace Cyclops.MainApplication.Configuration
{
    public class Profile : ViewModelBaseEx
    {
        private List<ConferencesServiceItem> friendlyConferencesServices;
        private ConnectionConfig connectionConfig;
        private bool autoLogin;
        private string name;

        private string theme;

        public Profile()
        {
            Theme = "Default";
            ConnectionConfig = new ConnectionConfig();
            FriendlyConferencesServices = new List<ConferencesServiceItem>();
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
                OnPropertyChanged();
            }
        }

        public bool AutoLogin
        {
            get { return autoLogin; }
            set
            {
                autoLogin = value;
                OnPropertyChanged();
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
                OnPropertyChanged();
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
                OnPropertyChanged();
            }
        }

        public List<ConferencesServiceItem> FriendlyConferencesServices
        {
            get { return friendlyConferencesServices; }
            set
            {
                friendlyConferencesServices = value;
                OnPropertyChanged();
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
