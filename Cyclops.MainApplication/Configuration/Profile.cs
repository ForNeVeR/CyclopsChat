using System.Collections.Generic;
using Cyclops.Core;
using Cyclops.Core.Configuration;
using GalaSoft.MvvmLight;

namespace Cyclops.MainApplication.Configuration
{
    public class Profile : ViewModelBase
    {
        private ConnectionConfig connectionConfig;
        private List<string> rooms;
        private string name;

        private string theme;

        public Profile()
        {
            Theme = "Default";

            //default rooms (not for "production")
            Rooms = new List<string>
                        {
                            "main@conference.jabber.uruchie.org",
                            "CIA@conference.jabber.uruchie.org",
                            //"dotnet@conference.jabber.ru",
                            //"windows@conference.jabber.ru",
                        };

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

        public List<string> Rooms
        {
            get { return rooms; }
            set
            {
                rooms = value;
                RaisePropertyChanged("Rooms");
            }
        }

        public override string ToString()
        {
            return Name;
        }
    }
}