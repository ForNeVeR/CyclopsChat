﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using Cyclops.MainApplication.Notifications;
using Cyclops.MainApplication.View;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    partial class MainViewModel
    {
        public RelayCommand OpenConferenceList { get; private set; }
        public RelayCommand CloseActiveConferenceOrPrivate { get; private set; }
        public RelayCommand UpdateStyles { get; private set; }
        public RelayCommand Quit { get; private set; }
        public RelayCommand ShowOrHide { get; private set; }

        private void InitializeCommands()
        {
            UpdateStyles = new RelayCommand(OnUpdateStyles);
            OpenConferenceList = new RelayCommand(OpenConferenceListAction, OpenConferenceListCanExecute);
            CloseActiveConferenceOrPrivate = new RelayCommand(CloseActiveConferenceOrPrivateAction, CloseActiveConferenceOrPrivateCanExecute);
            Quit = new RelayCommand(() => App.Current.Shutdown());
            ShowOrHide = new RelayCommand(ShowOrHideAction);
        }

        private void ShowOrHideAction()
        {
            TrayController.HideOrShowWindow(Application.Current.MainWindow);
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
