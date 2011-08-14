using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Cyclops.MainApplication.View;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    partial class MainViewModel
    {
        public RelayCommand OpenConferenceList { get; private set; }
        public RelayCommand CloseActiveConferenceOrPrivate { get; private set; }
        public RelayCommand AddToBookmarks { get; private set; }
        public RelayCommand RemoveFromBookmarks { get; private set; }
        public RelayCommand UpdateStyles { get; private set; }

        private void InitializeCommands()
        {
            UpdateStyles = new RelayCommand(OnUpdateStyles);
            OpenConferenceList = new RelayCommand(OpenConferenceListAction, OpenConferenceListCanExecute);
            AddToBookmarks = new RelayCommand(AddToBookmarksAction, AddToBookmarksCanExecute);
            RemoveFromBookmarks = new RelayCommand(RemoveFromBookmarksAction, RemoveFromBookmarksCanExecute);
            CloseActiveConferenceOrPrivate = new RelayCommand(CloseActiveConferenceOrPrivateAction, CloseActiveConferenceOrPrivateCanExecute);
        }

        private void AddToBookmarksAction()
        {
        }

        private bool AddToBookmarksCanExecute()
        {
            return true;
        }

        private void RemoveFromBookmarksAction()
        {
        }

        private bool RemoveFromBookmarksCanExecute()
        {
            return true;
        }

        private void CloseActiveConferenceOrPrivateAction()
        {
            if (SelectedConference != null)
                SelectedConference.Conference.LeaveAndClose();
            if (SelectedPrivate != null)
                PrivateViewModels.Remove(SelectedPrivate);
        }

        private bool CloseActiveConferenceOrPrivateCanExecute()
        {
            return SelectedConference != null || SelectedPrivate != null;
        }

        private static void OpenConferenceListAction()
        {
            //TODO: implemnt cache
            var dlg = new ConferencesList();
            dlg.Owner = Application.Current.MainWindow;
            dlg.ShowDialog();
        }

        private bool OpenConferenceListCanExecute()
        {
            return Session.IsAuthenticated;
        }

        private static void OnUpdateStyles()
        {
            ThemeManager.ApplyDefault();
        }
    }
}
