using System;
using Cyclops.Core;
using Cyclops.MainApplication.View.Dialogs;
using Cyclops.Xmpp.Data;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    public class NickAndStatusViewModel : ViewModelBaseEx
    {
        private readonly Action closeAction;
        private readonly IConference conference;
        private string nick;
        private string status;
        private StatusType statusType;

        public NickAndStatusViewModel(IConference conference, Action closeAction)
        {
            this.conference = conference;
            this.closeAction = closeAction;
            Ok = new RelayCommand(OkAction, OkCanExecute);
            Cancel = new RelayCommand(CancelAction);
            ChangeCurrentUserVcard = new RelayCommand(ChangeCurrentUserVcardAction, ChangeCurrentUserVcardCanExecute);

            Nick = conference.ConferenceId.Resource;
            Status = conference.Session.Status;
            StatusType = conference.Session.StatusType;
        }

        public RelayCommand Ok { get; set; }
        public RelayCommand Cancel { get; set; }
        public RelayCommand ChangeCurrentUserVcard { get; set; }

        public string Nick
        {
            get { return nick; }
            set
            {
                nick = value;
                RaisePropertyChanged("Nick");
            }
        }

        public string Status
        {
            get { return status; }
            set
            {
                status = value;
                RaisePropertyChanged("Status");
            }
        }

        public StatusType StatusType
        {
            get { return statusType; }
            set
            {
                statusType = value;
                RaisePropertyChanged("StatusType");
            }
        }

        private bool ChangeCurrentUserVcardCanExecute()
        {
            return conference.IsInConference && conference.Session.IsAuthenticated;
        }

        private void ChangeCurrentUserVcardAction()
        {
            DialogManager.ShowUsersVcard(conference.ConferenceId, false);
        }

        private void CancelAction()
        {
            closeAction();
        }

        private bool OkCanExecute()
        {
            return
                conference.IsInConference && conference.Session.IsAuthenticated &&
                (Nick != conference.ConferenceId.Resource ||
                 Status != conference.Session.Status ||
                 StatusType != conference.Session.StatusType);
        }

        private void OkAction()
        {
            conference.ChangeNickAndStatus(Nick, StatusType, Status);
            closeAction();
        }
    }
}
