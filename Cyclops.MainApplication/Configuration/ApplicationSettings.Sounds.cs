using System;

namespace Cyclops.MainApplication.Configuration
{
    partial class ApplicationSettings
    {
        private bool disableAllSounds;
        private bool soundEvenIfActive;
        private string soundOnIncomingPrivate;
        private string soundOnIncomingPublic;
        private string soundOnStatusChange;
        private string soundOnSystemMessage;
        private string soundOnUserJoin;
        private string soundOnUserLeave;

        public string SoundOnUserLeave
        {
            get { return soundOnUserLeave; }
            set
            {
                soundOnUserLeave = value;
                RaisePropertyChanged("SoundOnUserLeave");
            }
        }

        public string SoundOnUserJoin
        {
            get { return soundOnUserJoin; }
            set
            {
                soundOnUserJoin = value;
                RaisePropertyChanged("SoundOnUserJoin");
            }
        }

        public string SoundOnIncomingPrivate
        {
            get { return soundOnIncomingPrivate; }
            set
            {
                soundOnIncomingPrivate = value;
                RaisePropertyChanged("SoundOnIncomingPrivate");
            }
        }

        public string SoundOnIncomingPublic
        {
            get { return soundOnIncomingPublic; }
            set
            {
                soundOnIncomingPublic = value;
                RaisePropertyChanged("SoundOnIncomingPublic");
            }
        }

        public string SoundOnSystemMessage
        {
            get { return soundOnSystemMessage; }
            set
            {
                soundOnSystemMessage = value;
                RaisePropertyChanged("SoundOnSystemMessage");
            }
        }

        public string SoundOnStatusChange
        {
            get { return soundOnStatusChange; }
            set
            {
                soundOnStatusChange = value;
                RaisePropertyChanged("SoundOnStatusChange");
            }
        }

        public bool DisableAllSounds
        {
            get { return disableAllSounds; }
            set
            {
                disableAllSounds = value;
                RaisePropertyChanged("DisableAllSounds");
            }
        }

        public bool SoundEvenIfActive
        {
            get { return soundEvenIfActive; }
            set
            {
                soundEvenIfActive = value;
                RaisePropertyChanged("SoundEvenIfActive");
            }
        }

        private void CloneSoundsProperties(ApplicationSettings cloneObj)
        {
            cloneObj.SoundOnUserLeave = SoundOnUserLeave;
            cloneObj.SoundOnUserJoin = SoundOnUserJoin;
            cloneObj.SoundOnIncomingPrivate = SoundOnIncomingPrivate;
            cloneObj.SoundOnIncomingPublic = SoundOnIncomingPublic;
            cloneObj.SoundOnSystemMessage = SoundOnSystemMessage;
            cloneObj.SoundOnStatusChange = SoundOnStatusChange;
            cloneObj.DisableAllSounds = DisableAllSounds;
            cloneObj.SoundEvenIfActive = SoundEvenIfActive;
        }

        private void SetSoundsDefaultValues()
        {
        }
    }
}