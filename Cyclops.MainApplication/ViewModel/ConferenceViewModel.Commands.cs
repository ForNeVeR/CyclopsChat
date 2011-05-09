using System;
using Cyclops.MainApplication.View.Dialogs;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    public partial class ConferenceViewModel
    {
        public RelayCommand SendMessage { get; private set; }
        public RelayCommand GetUserVcard { get; private set; }
        public RelayCommand ChangeSubject { get; private set; }
        public RelayCommand ChangeCurrentUserVcard { get; private set; }
        public RelayCommand StartPrivateWithSelectedMember { get; private set; }
        public RelayCommand SendPublicMessageToSelectedMember { get; private set; }

        public void InitializeCommands()
        {
            SendMessage = new RelayCommand(OnSendMessage, () => !string.IsNullOrEmpty(CurrentlyTypedMessage));
            StartPrivateWithSelectedMember = new RelayCommand(StartPrivateWithSelectedMemberAction, DefaultCanExecuteMethod);
            GetUserVcard = new RelayCommand(GetUserVcardAction, DefaultCanExecuteMethod);
            ChangeCurrentUserVcard = new RelayCommand(() => DialogManager.ShowUsersVcard(Conference.ConferenceId, false), () => Conference.IsInConference);
            SendPublicMessageToSelectedMember = new RelayCommand(SendPublicMessageToMemberAction, DefaultCanExecuteMethod);
            ChangeSubject = new RelayCommand(ChangeSubjectAction, () => Conference.IsInConference);
        }

        private void GetUserVcardAction()
        {
            DialogManager.ShowUsersVcard(SelectedMember.ConferenceUserId, !Conference.ConferenceId.Equals(SelectedMember.ConferenceUserId));
        }

        private void SendPublicMessageToMemberAction()
        {
            SendPublicMessageToUser(SelectedMember.Nick + ": ");
        }

        private void ChangeSubjectAction()
        {
            DialogManager.ShowStringInputDialog(Localization.Conference.ChangeSubject, Conference.Subject,
                subj => Conference.ChangeSubject(subj), subj => subj != null && subj.Length < 1000);
        }

        private void StartPrivateWithSelectedMemberAction()
        {
            if (SelectedMember != null)
                ChatObjectFactory.GetSession().StartPrivate(SelectedMember.ConferenceUserId);
        }

        private bool DefaultCanExecuteMethod()
        {
            return SelectedMember != null && SelectedMember.ConferenceUserId != null &&
                   Conference != null && Conference.IsInConference;
        }

    }
}