using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops.Core.Smiles;
using Cyclops.MainApplication.Configuration;
using GalaSoft.MvvmLight;

namespace Cyclops.MainApplication
{
    public class ApplicationContext : ViewModelBase
    {
        #region Singleton implementation
        private ApplicationContext()
        {
            SmilePacks = new ISmilePack[0];
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
    }
}
