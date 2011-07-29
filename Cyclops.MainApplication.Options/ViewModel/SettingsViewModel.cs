using System;
using Cyclops.MainApplication.Localization;
using Cyclops.MainApplication.Options.Model;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.Options.ViewModel
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
        }
        
        private void CancelAction()
        {
            cancelAction();
        }

        private void CommitAction()
        {
            acceptAction();
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
