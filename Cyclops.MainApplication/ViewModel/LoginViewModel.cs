using System;
using System.Collections.Generic;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using Cyclops.Core;
using Cyclops.Core.Configuration;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Resource;
using Cyclops.Core.Security;
using Cyclops.MainApplication.Configuration;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private bool isBusy;
        private string name;
        private string server;
        private string errorMessage;
        private IUserSession session;
        private IEnumerable<Profile> profiles;

        public LoginViewModel()
        {
            if (IsInDesignMode)
                return;

            Server = ConfigurationManager.AppSettings["DefaultServer"];

            //profiles = ProfileManager.GetSavedProfiles();
            Session = ChatObjectFactory.GetSession();
            Session.Authenticated += SessionAuthenticated;
            Session.ConnectionDropped += SessionConnectionDropped;
            Authenticate = new RelayCommand<PasswordBox>(AuthenticateAction, AuthenticateCanExecute);

            //QUICK:
            //Name = "nagg";
            //AuthenticateAction(new PasswordBox { Password = "" }); //черт, запалил пасс :) но уже сменил
        }

        public RelayCommand<PasswordBox> Authenticate { get; set; }

        public string Name
        {
            get { return name; }
            set
            {
                name = value;
                RaisePropertyChanged("Name");
            }
        }

        public string Server
        {
            get { return server; }
            set
            {
                server = value;
                RaisePropertyChanged("Server");
            }
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBusy");
            }
        }

        public string ErrorMessage
        {
            get { return errorMessage; }
            set
            {
                errorMessage = value;
                RaisePropertyChanged("ErrorMessage");
            }
        }

        public IUserSession Session
        {
            get { return session; }
            set
            {
                session = value;
                RaisePropertyChanged("Session");
            }
        }

        public event EventHandler Authenticated = delegate { };

        private void SessionConnectionDropped(object sender, AuthenticationEventArgs e)
        {
            IsBusy = false;
            ErrorMessage = e.ErrorMessage;
        }

        private void SessionAuthenticated(object sender, AuthenticationEventArgs e)
        {
            if (e.Success)
            {
                //load smiles into context
                ApplicationContext.Current.SmilePacks = ChatObjectFactory.GetSmilesManager().GetSmilePacks();
                IsBusy = false;

#if DEBUG
                ChatObjectFactory.ShowDebugWindow();
#endif
                Authenticated(this, e);
            }
            else
            {
                IsBusy = false;
                ErrorMessage = e.ErrorMessage;
            }
        }

        private bool AuthenticateCanExecute(PasswordBox passwordBox)
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !string.IsNullOrWhiteSpace(Server) &&
                   !Name.Contains("@") && !Name.Contains("/");
        }
        
        private void AuthenticateAction(PasswordBox passwordBox)
        {
            ErrorMessage = string.Empty;

            if (string.IsNullOrEmpty(passwordBox.Password))
            {
                ErrorMessage = "Password can't be empty";
                return;
            }
            
            IsBusy = true;
            string encodedPsw = ChatObjectFactory.GetStringEncryptor().EncryptString(passwordBox.Password);
            var connectionConfig = new ConnectionConfig
                                       {
                                           EncodedPassword = encodedPsw,
                                           Server = Server,
                                           User = Name,
                                       };

            ApplicationContext.Current.CurrentProfile = new Profile
                                                            {
                                                                Name = Name, 
                                                                ConnectionConfig = connectionConfig
                                                            };

            Session.AuthenticateAsync(connectionConfig);
        }

        protected override void Dispose(bool disposing)
        {
            Session.Authenticated -= SessionAuthenticated;
            Session.ConnectionDropped -= SessionConnectionDropped;
            base.Dispose(disposing);
        }
    }
}