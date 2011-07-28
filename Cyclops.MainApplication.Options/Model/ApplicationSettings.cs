using System;
using Cyclops.MainApplication.Options.Helpers;
using GalaSoft.MvvmLight;

namespace Cyclops.MainApplication.Options.Model
{
    public partial class ApplicationSettings : ViewModelBase, ICloneable
    {
        #region Implementation of ICloneable

        /// <summary>
        /// Creates a new object that is a copy of the current instance.
        /// </summary>
        public object Clone()
        {
            return CreateCopy();
        }

        public ApplicationSettings CreateCopy()
        {
            var cloneObj = new ApplicationSettings();
            CloneInterfaceProperties(cloneObj);
            CloneCommonProperties(cloneObj);
            CloneSoundsProperties(cloneObj);
            CloneStatusProperties(cloneObj);
            return cloneObj;
        }

        public void SetDefaultValues()
        {
            SetInterfaceDefaultValues();
            SetCommonDefaultValues();
            SetSoundsDefaultValues();
            SetStatusDefaultValues();
        }

        private static readonly Serializer<ApplicationSettings> Serializer = new Serializer<ApplicationSettings>();
        public const string ConfigFilePath = @"Data\Profiles\Application.config.xml";

        public static void Save(ApplicationSettings obj)
        {
            Serializer.Serialize(obj, ConfigFilePath);
        }

        public static ApplicationSettings Load()
        {
            return Serializer.Deserialize(ConfigFilePath);
        }

        public void Save()
        {
            Save(this);
        }
        
        #endregion
    }
}
