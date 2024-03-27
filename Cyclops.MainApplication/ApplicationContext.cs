using System;
using System.Diagnostics;
using System.Windows.Threading;
using Cyclops.Core;
using Cyclops.Core.Smiles;
using Cyclops.MainApplication.Configuration;
using Cyclops.MainApplication.Helpers;
using Cyclops.MainApplication.Localization;
using Cyclops.MainApplication.Options.Model;
using Cyclops.MainApplication.ViewModel;
using Cyclops.Windows;
using Cyclops.Xmpp.Data;
using static Cyclops.Wpf.DesignerUtil;

namespace Cyclops.MainApplication
{
    public class ApplicationContext : ViewModelBaseEx
    {
        private readonly LastInputDetector lastInputDetector = new(
            pollInterval: TimeSpan.FromSeconds(2.0),
            idleInterval: TimeSpan.FromSeconds(60.0));

        #region Singleton implementation
        private ApplicationContext()
        {
            SmilePacks = new ISmilePack[0];
            if (IsInDesignMode)
                return;
            ReloadApplicationSettings();
            DisableAllSounds = ApplicationSettings.DisableAllSounds;

            var dispatcher = Dispatcher.CurrentDispatcher;
            lastInputDetector.IdleModePeriodic += (_, idleTime) =>
                dispatcher.InvokeAsync(() => OnIdleModePeriodic(idleTime));
            lastInputDetector.LeaveIdleMode += (_, _) => dispatcher.InvokeAsync(OnLeaveIdleMode);
        }

        protected override void OnDeactivated()
        {
            base.OnDeactivated();
            lastInputDetector.Dispose();
        }

        private bool originalStatusSaved = false;
        private StatusType originalStatus;

        private void OnIdleModePeriodic(TimeSpan span)
        {
            if (!Session.IsAuthenticated)
                return;

            if (Session.StatusType != StatusType.ExtendedAway && Session.StatusType != StatusType.Away)
            {
                originalStatusSaved = true;
                originalStatus = Session.StatusType;
            }

            Trace.WriteLine((int)span.TotalSeconds);

            if (Session.StatusType == StatusType.ExtendedAway)
                return;

            int idleMinutes = (int)span.TotalMinutes;
            Trace.WriteLine(idleMinutes + "minutes");

            int awayAfter = Settings.AutoAwayAfter;
            if (awayAfter > 0 && idleMinutes >= awayAfter && Session.StatusType != StatusType.Away)
                Session.StatusType = StatusType.Away;

            int naAfter = Settings.AutoExtendedAwayAfter;
            if (naAfter > 0 && idleMinutes >= naAfter)
                Session.StatusType = StatusType.ExtendedAway;
        }

        private void OnLeaveIdleMode()
        {
            if (!Session.IsAuthenticated)
                return;

            if (Session.StatusType != originalStatus && originalStatusSaved)
            {
                Session.StatusType = originalStatus;
            }
            originalStatusSaved = false;
        }

        private static ApplicationContext instance = null;
        public static ApplicationContext Current
        {
            get
            {
                if (instance == null)
                    instance = new ApplicationContext();
                return instance;
            }
        }

        #endregion

        private ISmilePack[] smilePacks;

        /// <summary>
        /// Loaded smiles
        /// </summary>
        public ISmilePack[] SmilePacks
        {
            get { return smilePacks; }
            set
            {
                smilePacks = value;
                OnPropertyChanged();
            }
        }

        public IUserSession Session => ChatObjectFactory.GetSession();

        private Profile currentProfile;

        /// <summary>
        /// Current user profile
        /// </summary>
        public Profile CurrentProfile
        {
            get { return currentProfile; }
            set
            {
                currentProfile = value;
                OnPropertyChanged();
            }
        }

        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel
        {
            get { return mainViewModel; }
            set
            {
                mainViewModel = value;
                OnPropertyChanged();
            }
        }

        private ApplicationSettings applicationSettings;
        public ApplicationSettings ApplicationSettings
        {
            get { return applicationSettings; }
            set
            {
                applicationSettings = value;
                OnPropertyChanged();
            }
        }

        public void ReloadApplicationSettings()
        {
            ApplicationSettings = ApplicationSettings.Load();
            SystemHelper.SetStartup(ApplicationSettings.StartWithWindows);
            LocalizationManager.ChangeLanguage(ApplicationSettings.SelectedLanguage);
        }

        #region Global nonsavable options

        private bool disableAllSounds;
        public bool DisableAllSounds
        {
            get { return disableAllSounds; }
            set
            {
                disableAllSounds = value;
                OnPropertyChanged();
            }
        }

        #endregion
    }
}
