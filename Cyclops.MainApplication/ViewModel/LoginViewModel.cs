using System;
using System.Configuration;
using System.Windows;
using System.Windows.Controls;
using Cyclops.Core;
using Cyclops.Core.Configuration;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Security;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    public class LoginViewModel : ViewModelBase
    {
        private bool isBusy;
        private string name;
        private IUserSession session;

        public LoginViewModel()
        {
            if (IsInDesignMode)
                return;

            Session = ChatObjectFactory.GetSession();
            Session.Authenticated += SessionAuthenticated;
            Session.ConnectionDropped += SessionConnectionDropped;
            Authenticate = new RelayCommand<PasswordBox>(AuthenticateAction, AuthenticateCanExecute);

            //DEBUG:
            //Name = "cyclops";
            //AuthenticateAction(new PasswordBox { Password = "cyclops" });
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

        public bool IsBusy
        {
            get { return isBusy; }
            set
            {
                isBusy = value;
                RaisePropertyChanged("IsBusy");
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
            MessageBox.Show(e.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void SessionAuthenticated(object sender, AuthenticationEventArgs e)
        {
            if (e.Success)
            {
                //load smiles into context
                ApplicationContext.Current.SmilePacks = ChatObjectFactory.GetSmilesManager().GetSmilePacks();
                IsBusy = false;
                Authenticated(this, e);
            }
            else
            {
                IsBusy = false;
                MessageBox.Show(e.ErrorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private bool AuthenticateCanExecute(PasswordBox passwordBox)
        {
            return !string.IsNullOrWhiteSpace(Name) &&
                   !Name.Contains("@") && !Name.Contains("/");
        }

        private void AuthenticateAction(PasswordBox passwordBox)
        {
            IsBusy = true;
            string encodedPsw =
                ChatObjectFactory.ServiceLocator.GetInstance<IStringEncryptor>().EncryptString(passwordBox.Password);
            Session.AuthenticateAsync(new ConnectionConfig
                                          {
                                              EncodedPassword = encodedPsw,
                                              Server = ConfigurationManager.AppSettings["DefaultServer"],
                                              User = Name,
                                          });
        }

        protected override void Dispose(bool disposing)
        {
            Session.Authenticated -= SessionAuthenticated;
            Session.ConnectionDropped -= SessionConnectionDropped;
            base.Dispose(disposing);
        }
    }
}