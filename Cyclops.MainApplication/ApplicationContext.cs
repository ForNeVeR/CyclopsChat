using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Threading;
using Cyclops.MainApplication.Options.Model;
using bedrock.util;
using Cyclops.Core;
using Cyclops.Core.CustomEventArgs;
using Cyclops.Core.Resource;
using Cyclops.Core.Smiles;
using Cyclops.MainApplication.Configuration;
using Cyclops.MainApplication.Helpers;
using Cyclops.MainApplication.ViewModel;
using GalaSoft.MvvmLight;

namespace Cyclops.MainApplication
{
    public class ApplicationContext : ViewModelBaseEx
    {
        private IdleTime idleTime = null;

        #region Singleton implementation
        private ApplicationContext()
        {
            SmilePacks = new ISmilePack[0];
            ReloadApplicationSettings();
            DisableAllSounds = ApplicationSettings.DisableAllSounds;

            idleTime = new IdleTime(2, 60);
            idleTime.InvokeControl = new SynchronizeInvokeImpl(Dispatcher.CurrentDispatcher);
            idleTime.OnIdle += IdleTimeOnIdle;
            idleTime.OnUnIdle += IdleTimeOnUnIdle;
        }

        private bool originalStatusSaved = false;
        private StatusType originalStatus;

        private void IdleTimeOnIdle(object sender, TimeSpan span)
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

        private void IdleTimeOnUnIdle(object sender, TimeSpan span)
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
                RaisePropertyChanged("SmilePacks");
            }
        }

        public IUserSession Session
        {
            get { return ChatObjectFactory.GetSession(); }
        }

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
                RaisePropertyChanged("CurrentProfile");
            }
        }

        private MainViewModel mainViewModel;
        public MainViewModel MainViewModel
        {
            get { return mainViewModel; }
            set
            {
                mainViewModel = value;
                RaisePropertyChanged("MainViewModel");
            }
        }

        private ApplicationSettings applicationSettings;
        public ApplicationSettings ApplicationSettings
        {
            get { return applicationSettings; }
            set
            {
                applicationSettings = value;
                RaisePropertyChanged("ApplicationSettings");
            }
        }

        public void ReloadApplicationSettings()
        {
            ApplicationSettings = ApplicationSettings.Load();
            SystemHelper.SetStartup(ApplicationSettings.StartWithWindows);
            Localization.LocalizationManager.ChangeLanguage(ApplicationSettings.SelectedLanguage);
        }

        #region Global nonsavable options

        private bool disableAllSounds;
        public bool DisableAllSounds
        {
            get { return disableAllSounds; }
            set
            {
                disableAllSounds = value;
                RaisePropertyChanged("DisableAllSounds");
            }
        }

        #endregion
    }
}
