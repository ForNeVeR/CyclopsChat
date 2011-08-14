using System.ComponentModel;

namespace Cyclops.MainApplication.Localization
{
    /// <summary>
    /// This wrapper allows to change language directly in runtime (including XAML binding)
    /// </summary>
    public class ResourceWrapper : INotifyPropertyChanged
    //EX: Text="{Binding Path=ApplicationStrings.ApplicationName, Source={StaticResource ResourceWrapper}}"
    {

        private Conference conference = new Conference();
        public Localization.Conference Conference
        {
            get { return conference; }
            set
            {
                conference = value;
                OnPropertyChanged("Conference");
            }
        }

        private Login login = new Login();
        public Localization.Login Login
        {
            get { return login; }
            set
            {
                login = value;
                OnPropertyChanged("Login");
            }
        }

        private ContextMenus contextMenu = new ContextMenus();
        public Localization.ContextMenus ContextMenus
        {
            get { return contextMenu; }
            set
            {
                contextMenu = value;
                OnPropertyChanged("ContextMenus");
            }
        }

        private Common common = new Common();
        public Common Common
        {
            get { return common; }
            set
            {
                common = value;
                OnPropertyChanged("Common");
            }
        }

        private Vcard vcard = new Vcard();
        public Vcard Vcard
        {
            get { return vcard; }
            set
            {
                vcard = value;
                OnPropertyChanged("Vcard");
            }
        }
        
        private ConferenceList conferenceList = new ConferenceList();
        public Localization.ConferenceList ConferenceList
        {
            get { return conferenceList; }
            set
            {
                conferenceList = value;
                OnPropertyChanged("ConferenceList");
            }
        }

        private Main main = new Main();
        public Localization.Main Main
        {
            get { return main; }
            set
            {
                main = value;
                OnPropertyChanged("Main");
            }
        }

        private Settings settings = new Settings();
        public Localization.Settings Settings
        {
            get { return settings; }
            set
            {
                settings = value;
                OnPropertyChanged("Settings");
            }
        }

        public void Refresh()
        {
            Login = new Login();
            Conference = new Conference();
            Main = new Main();
            ConferenceList = new ConferenceList();
            Common = new Common();
            Vcard = new Vcard();
            Settings = new Settings();
            ContextMenus = new ContextMenus();
        }

        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(name));
        }
    }
}
