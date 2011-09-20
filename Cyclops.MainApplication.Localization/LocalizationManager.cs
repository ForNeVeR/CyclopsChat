using System;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Cyclops.MainApplication.Localization
{
    public class LocalizationManager
    {
        public static void ChangeLanguage(string culture)
        {
            if (string.IsNullOrWhiteSpace(culture))
                culture = CultureInfo.CurrentCulture.Name;

            if (Thread.CurrentThread.CurrentUICulture.Name.Equals(culture))
                return;

            Thread.CurrentThread.CurrentUICulture = new CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = new CultureInfo(culture);
            var rw = Application.Current.Resources["ResourceWrapper"] as ResourceWrapper;
            if (rw != null)
                rw.Refresh();
        }

        public static void ToRussian()
        {
            ChangeLanguage("ru-RU");
        }

        public static void ToEnglish()
        {
            ChangeLanguage("en-US");
        }

        public Language[] Languages { get; set; }

        private Language selectedLanguage;
        public Language SelectedLanguage
        {
            get { return selectedLanguage; }
            set
            {
                selectedLanguage = value;
                if (value != null)
                    ChangeLanguage(value.Culture);
            }
        }

        private LocalizationManager()
        {
            Languages = new[]
                            {
                                new Language {Culture = "en-US", Name = "English"},
                                new Language {Culture = "ru-RU", Name = "Russian"}
                            };
            var lang = Languages.First();
            SelectedLanguage = lang;
        }

        private static LocalizationManager instance = null;
        public static LocalizationManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new LocalizationManager();
                return instance;
            }
        }
            
    }
}
