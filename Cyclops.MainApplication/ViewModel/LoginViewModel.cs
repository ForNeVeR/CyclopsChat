using System;
using System.Configuration;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Controls;
using Cyclops.Configuration;
using Cyclops.Core;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Helpers;
using Cyclops.MainApplication.Configuration;
using Cyclops.Xmpp.Registration;
using GalaSoft.MvvmLight.CommandWpf;

namespace Cyclops.MainApplication.ViewModel;

public class LoginViewModel : ViewModelBaseEx
{
    private readonly ILogger logger;

    private bool isBusy;
    private string name;
    private string server;
    private string? errorMessage;
    private IUserSession session;
    private bool autoLogin;
    private readonly Profile profile;
    private readonly IRegistrationManager registrationManager;
    private string password;

    private LoginViewModel(ILogger logger)
    {
        this.logger = logger;
        if (IsInDesignMode)
            return;

        profile = ApplicationContext.Current.CurrentProfile = ProfileManager.LoadProfile();

        registrationManager = ChatObjectFactory.GetRegistrationManager();
        Server = profile.ConnectionConfig.Server;
        Name = profile.ConnectionConfig.User;
        AutoLogin = profile.AutoLogin;

        Session = ChatObjectFactory.GetSession();
        Session.AutoReconnect = false;
        Session.Authenticated += SessionAuthenticated;
        Session.ConnectionDropped += SessionConnectionDropped;
        Authenticate = new RelayCommand<PasswordBox>(AuthenticateAction, AuthenticateCanExecute);
        Register = new RelayCommand<PasswordBox>(RegisterAction, RegisterCanExecute);

        if (AutoLogin)
        {
            try
            {
                var decodedPassword = ChatObjectFactory.GetStringEncryptor().DecryptString(profile.ConnectionConfig.EncodedPassword);
                AuthenticateAction(new PasswordBox { Password = decodedPassword });
            }
            catch
            {
                //TODO: log an exception
            }
        }
    }

    public LoginViewModel() : this(ChatObjectFactory.GetDebugLogger()) {}

    public string ApplicationVersion => Assembly.GetExecutingAssembly().GetName().Version?.ToString(3) ?? "N/A";

    private bool RegisterCanExecute(PasswordBox obj)
    {
        return Validate();
    }

    private bool ValidatePassword(PasswordBox passwordBox)
    {
        ErrorMessage = string.Empty;
        if (string.IsNullOrEmpty(passwordBox.Password))
        {
            ErrorMessage = Localization.Login.EmptyPassword;
            return false;
        }
        return true;
    }

    private void RegisterAction(PasswordBox passwordBox)
    {
        if (!ValidatePassword(passwordBox))
            return;

        IsBusy = true;
        password = passwordBox.Password;

        DoRegister().NoAwait(logger);
        async Task DoRegister()
        {
            RegistrationResult registrationResult;
            try
            {
                registrationResult = await registrationManager.RegisterNewUser(new ConnectionConfig
                {
                    Server = Server,
                    User = Name,
                    EncodedPassword = ChatObjectFactory.GetStringEncryptor().EncryptString(password),
                });
            }
            catch (Exception ex)
            {
                logger.LogError("Exception during registration.", ex);
                registrationResult = new RegistrationResult(ex.Message);
            }

            ErrorMessage = registrationResult.ErrorMessage;
            if (!registrationResult.HasError)
                AuthenticateAction(new PasswordBox {Password = password});
            else
                IsBusy = false;
        }
    }

    public RelayCommand<PasswordBox> Authenticate { get; set; }
    public RelayCommand<PasswordBox> Register { get; set; }

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

    public bool AutoLogin
    {
        get { return autoLogin; }
        set
        {
            autoLogin = value;
            RaisePropertyChanged("AutoLogin");
        }
    }

    public string? ErrorMessage
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
            Session.AutoReconnect = true;
            //load smiles into context
            ApplicationContext.Current.SmilePacks = ChatObjectFactory.GetSmilesManager().GetSmilePacks();
            IsBusy = false;

            if (ConfigurationManager.AppSettings["ShowXmppConsole"].StringToBool())
                ChatObjectFactory.ShowDebugWindow();
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
        return Validate();
    }

    private bool Validate()
    {
        return !string.IsNullOrWhiteSpace(Name) &&
               !string.IsNullOrWhiteSpace(Server) &&
               !Name.Contains("@") && !Name.Contains("/");
    }

    private void AuthenticateAction(PasswordBox passwordBox)
    {
        if (!ValidatePassword(passwordBox))
            return;

        IsBusy = true;
        string encodedPsw = ChatObjectFactory.GetStringEncryptor().EncryptString(passwordBox.Password);
        var connectionConfig = new ConnectionConfig
        {
            EncodedPassword = encodedPsw,
            Server = Server,
            User = Name,
            NetworkHost = profile.ConnectionConfig.NetworkHost,
            Port = profile.ConnectionConfig.Port
        };
        profile.AutoLogin = AutoLogin;
        profile.ConnectionConfig = connectionConfig;
        ProfileManager.SaveProfile(profile);
        Session.AuthenticateAsync(connectionConfig);
    }

    public override void Cleanup()
    {
        Session.Authenticated -= SessionAuthenticated;
        Session.ConnectionDropped -= SessionConnectionDropped;
    }
}
