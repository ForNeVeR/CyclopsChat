using System;
using Cyclops.MainApplication.Configuration;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    public class SettingsViewModel : ApplicationSettings
    {
        private readonly Action acceptAction = null;
        private readonly Action cancelAction = null;

        public SettingsViewModel()
        {
        }

        public SettingsViewModel(Action acceptAction, Action cancelAction)
        {
            if (IsInDesignMode)
                return;

            this.acceptAction = acceptAction;
            this.cancelAction = cancelAction;

            Commit = new RelayCommand(CommitAction);
            Cancel = new RelayCommand(CancelAction);

            LoadSettings();
        }
        
        private void CancelAction()
        {
            cancelAction();
        }

        private void CommitAction()
        {
            if (SaveSettings())
                acceptAction();
        }

        private void LoadSettings()
        {
            ApplicationContext.Current.ReloadApplicationSettings();
            Settings = ApplicationContext.Current.ApplicationSettings.CreateCopy();
        }

        private bool SaveSettings()
        {
            ApplicationContext.Current.ApplicationSettings = Settings.CreateCopy();
            ApplicationContext.Current.ApplicationSettings.Save();
            ApplicationContext.Current.ReloadApplicationSettings();
            return true;
        }

        public RelayCommand Commit { get; set; }
        public RelayCommand Cancel { get; set; }

        private ApplicationSettings settings;
        public ApplicationSettings Settings
        {
            get { return settings; }
            set
            {
                settings = value;
                RaisePropertyChanged("Settings");
            }
        }
    }
}
