using System.Collections.Generic;
using Cyclops.Core;
using Cyclops.Core.Configuration;
using Cyclops.MainApplication.ViewModel;
using GalaSoft.MvvmLight;

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
                RaisePropertyChanged("Name");
            }
        }

        public bool AutoLogin
        {
            get { return autoLogin; }
            set
            {
                autoLogin = value;
                RaisePropertyChanged("AutoLogin");
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
        
        public List<ConferencesServiceItem> FriendlyConferencesServices
        {
            get { return friendlyConferencesServices; }
            set
            {
                friendlyConferencesServices = value;
                RaisePropertyChanged("FriendlyConferencesServices");
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}