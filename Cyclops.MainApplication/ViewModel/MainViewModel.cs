using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using Cyclops.Core;
using Cyclops.Core.Resource;
using Cyclops.MainApplication.Controls;
using Cyclops.MainApplication.View;
using Cyclops.MainApplication.View.Popups;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    /// <summary>
    /// </summary>
    public partial class MainViewModel : ViewModelBaseEx, ISessionHolder
    {
        private readonly IMainView mainView;
        private ObservableCollection<ConferenceViewModel> conferencesModels;
        private bool isApplicationInActiveState;
        private ObservableCollection<PrivateViewModel> privateViewModels;
        private ConferenceViewModel selectedConference;

        private PrivateViewModel selectedPrivate;

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IMainView mainView)
        {
            this.mainView = mainView;
            if (IsInDesignMode)
                return;

            ApplicationContext.Current.MainViewModel = this;

            InitializeCommands();

            TrayController.Instance.ShowDefaultIcon();
            Session = ChatObjectFactory.GetSession();
            Session.ErrorMessageRecieved += SessionCommonError;

            ConferencesModels = new ObservableCollection<ConferenceViewModel>();
            PrivateViewModels = new ObservableCollection<PrivateViewModel>();

            string defaultNick = Session.CurrentUserId.User;

            foreach (var room in ApplicationContext.Current.CurrentProfile.Rooms)
            {
                var identity = IdentifierBuilder.Create(room);
                if (string.IsNullOrEmpty(identity.Resource))
                    identity = IdentifierBuilder.WithAnotherResource(identity, defaultNick);

                Session.OpenConference(identity);
            }


            Session.Conferences.SynchronizeWith(ConferencesModels, 
                conference => new ConferenceViewModel(null, conference), i => i, i => i.Conference);
            
            SubscribeToEvents();
            IsApplicationInActiveState = Application.Current.MainWindow.IsActive;
        }

        private static void SessionCommonError(object sender, Core.CustomEventArgs.ErrorEventArgs e)
        {
            NotificationManager.NotifyError(e.From.ToString(), e.Message);
        }
        
        private void SubscribeToEvents()
        {
            Session.PrivateMessages.CollectionChanged += PrivateMessagesCollectionChanged;
            Session.PublicMessage += SessionPublicMessage;
            Application.Current.MainWindow.Activated += (s, e) => IsApplicationInActiveState = true;
            Application.Current.MainWindow.Deactivated += (s, e) => IsApplicationInActiveState = false;
        }

        public void ShowSettings()
        {
            mainView.ShowSettings();
        }
    
        public ConferenceViewModel SelectedConference
        {
            get { return selectedConference; }
            set
            {
                selectedConference = value;
                RaisePropertyChanged("SelectedConference");
            }
        }

        public PrivateViewModel SelectedPrivate
        {
            get { return selectedPrivate; }
            set
            {
                selectedPrivate = value;
                RaisePropertyChanged("SelectedPrivate");
            }
        }


        public bool IsApplicationInActiveState
        {
            get { return isApplicationInActiveState; }
            set
            {
                isApplicationInActiveState = value;
                RaisePropertyChanged("IsApplicationInActiveState");
                if (value)
                    TrayController.Instance.StopBlink();
            }
        }

        /// <summary>
        /// Collection of currently opened conferences models
        /// </summary>
        public ObservableCollection<ConferenceViewModel> ConferencesModels
        {
            get { return conferencesModels; }
            set
            {
                conferencesModels = value;
                RaisePropertyChanged("ConferencesModels");
            }
        }

        public ObservableCollection<PrivateViewModel> PrivateViewModels
        {
            get { return privateViewModels; }
            set
            {
                privateViewModels = value;
                RaisePropertyChanged("PrivateViewModels");
            }
        }

        #region Implementation of IJabberSessionHolder

        private IUserSession session;

        /// <summary>
        /// Session object
        /// </summary>
        public IUserSession Session
        {
            get { return session; }
            private set
            {
                session = value;
                RaisePropertyChanged("Session");
            }
        }

        #endregion
        
        private void PrivateMessagesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                PrivateMessage msg = e.NewItems.OfType<PrivateMessage>().FirstOrDefault();
                if (msg != null)
                {
                    PrivateViewModel privateViewModel =
                        PrivateViewModels.FirstOrDefault(i => i.Participant.Equals(msg.AuthorId));
                    if (privateViewModel != null)
                    {
                        privateViewModel.Messages.Add(new MessageViewModel(msg));
                    }
                    else
                    {
                        var newPrivate = new PrivateViewModel(null) {Participant = msg.AuthorId};
                        newPrivate.Conference = msg.Conference;
                        if (msg.Body != null)
                            newPrivate.Messages.Add(new MessageViewModel(msg));
                        PrivateViewModels.Add(newPrivate);
                    }

                    if (!IsApplicationInActiveState)
                    {
                        TrayController.Instance.StartBlink();
                        NotificationManager.NotifyPrivate(msg, null);
                    }
                }
            }
        }

        private void SessionPublicMessage(object sender, EventArgs e)
        {
            if (!IsApplicationInActiveState && !Settings.BlinkOnlyOnPrivates)
                TrayController.Instance.StartBlink();
        }
    }
}