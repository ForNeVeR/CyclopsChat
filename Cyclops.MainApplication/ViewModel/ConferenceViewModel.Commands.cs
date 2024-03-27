using System.Windows;
using CommunityToolkit.Mvvm.Input;
using Cyclops.MainApplication.View.Dialogs;

namespace Cyclops.MainApplication.ViewModel
{
    public partial class ConferenceViewModel
    {
        public RelayCommand AddToBookmarks { get; private set; }
        public RelayCommand RemoveFromBookmarks { get; private set; }
        public RelayCommand GetUserVcard { get; private set; }
        public RelayCommand ChangeSubject { get; private set; }
        public RelayCommand ChangeCurrentUserVcard { get; private set; }
        public RelayCommand StartPrivateWithSelectedMember { get; private set; }
        public RelayCommand SendPublicMessageToSelectedMember { get; private set; }
        public RelayCommand NickAndStatusChange { get; private set; }

        public void InitializeCommands()
        {
            StartPrivateWithSelectedMember = new RelayCommand(StartPrivateWithSelectedMemberAction, DefaultCanExecuteMethod);
            GetUserVcard = new RelayCommand(GetUserVcardAction, DefaultCanExecuteMethod);
            ChangeCurrentUserVcard = new RelayCommand(() => DialogManager.ShowUsersVcard(Conference.ConferenceId, false), () => Conference.IsInConference);
            SendPublicMessageToSelectedMember = new RelayCommand(SendPublicMessageToMemberAction, DefaultCanExecuteMethod);
            ChangeSubject = new RelayCommand(ChangeSubjectAction, () => Conference.IsInConference);
            AddToBookmarks = new RelayCommand(AddToBookmarksAction, AddToBookmarksCanExecute);
            RemoveFromBookmarks = new RelayCommand(RemoveFromBookmarksAction, RemoveFromBookmarksCanExecute);
            NickAndStatusChange = new RelayCommand(NickAndStatusChangeAction, () => Conference.IsInConference && Conference.Session.IsAuthenticated);
        }

        private void NickAndStatusChangeAction()
        {
            NickAndStatusDialog dlg = new NickAndStatusDialog(Conference);
            dlg.Owner = Application.Current.MainWindow;
            dlg.ShowDialog();
        }

        private void RemoveFromBookmarksAction()
        {
            Conference.Session.RemoveFromBookmarks(Conference.ConferenceId);
        }

        private bool RemoveFromBookmarksCanExecute()
        {
            return Conference.IsInConference;
        }

        private void AddToBookmarksAction()
        {
            Conference.Session.AddToBookmarks(Conference.ConferenceId);
        }

        private bool AddToBookmarksCanExecute()
        {
            return Conference.IsInConference;
        }

        private void GetUserVcardAction()
        {
            DialogManager.ShowUsersVcard(SelectedMember.ConferenceUserId, !Conference.ConferenceId.Equals(SelectedMember.ConferenceUserId));
        }

        private void SendPublicMessageToMemberAction()
        {
            AppendText(SelectedMember.Nick + ": ");
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
