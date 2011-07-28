using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using Cyclops.MainApplication.Localization;

namespace Cyclops.MainApplication.Options.Model
{
    partial class ApplicationSettings
    {
        private bool blinkOnlyOnPrivates;
        private bool disableBlinking;
        private bool enableErrorPopups;
        private bool enablePrivatePopups;
        private bool isSmilesAnimated;
        private string selectedLanguage;
        private int smilesLimitInMessage;
        private int popupStaysOpenning;

        [XmlIgnore]
        public string[] Languages
        {
            get
            {
                return LocalizationManager.Instance.Languages.Select(i => i.Culture).ToArray();
            }
        }

        public string SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                RaisePropertyChanged("SelectedLanguage");
            }
        }

        public int SmilesLimitInMessage
        {
            get { return smilesLimitInMessage; }
            set
            {
                smilesLimitInMessage = value;
                RaisePropertyChanged("SmilesLimitInMessage");
            }
        }

        public bool EnablePrivatePopups
        {
            get { return enablePrivatePopups; }
            set
            {
                enablePrivatePopups = value;
                RaisePropertyChanged("EnablePrivatePopups");
            }
        }

        public bool EnableErrorPopups
        {
            get { return enableErrorPopups; }
            set
            {
                enableErrorPopups = value;
                RaisePropertyChanged("EnableErrorPopups");
            }
        }

        public int PopupStaysOpenning
        {
            get { return popupStaysOpenning; }
            set
            {
                popupStaysOpenning = value;
                RaisePropertyChanged("PopupStaysOpenning");
            }
        }

        public bool DisableBlinking
        {
            get { return disableBlinking; }
            set
            {
                disableBlinking = value;
                RaisePropertyChanged("DisableBlinking");
            }
        }

        public bool BlinkOnlyOnPrivates
        {
            get { return blinkOnlyOnPrivates; }
            set
            {
                blinkOnlyOnPrivates = value;
                RaisePropertyChanged("BlinkOnlyOnPrivates");
            }
        }

        public bool IsSmilesAnimated
        {
            get { return isSmilesAnimated; }
            set
            {
                isSmilesAnimated = value;
                RaisePropertyChanged("IsSmilesAnimated");
            }
        }

        private void CloneInterfaceProperties(ApplicationSettings cloneObj)
        {
            cloneObj.SelectedLanguage = SelectedLanguage;
            cloneObj.SmilesLimitInMessage = SmilesLimitInMessage;
            cloneObj.EnablePrivatePopups = EnablePrivatePopups;
            cloneObj.EnableErrorPopups = EnableErrorPopups;
            cloneObj.PopupStaysOpenning = PopupStaysOpenning;
            cloneObj.DisableBlinking = DisableBlinking;
            cloneObj.BlinkOnlyOnPrivates = BlinkOnlyOnPrivates;
            cloneObj.IsSmilesAnimated = IsSmilesAnimated;
        }

        private void SetInterfaceDefaultValues()
        {
            SelectedLanguage = CultureInfo.CurrentCulture.Name;
            SmilesLimitInMessage = 100;
            EnablePrivatePopups = true;
            EnableErrorPopups = true;
            PopupStaysOpenning = 3;
            DisableBlinking = false;
            BlinkOnlyOnPrivates = false;
            IsSmilesAnimated = false;
        }
    }
}