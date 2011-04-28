using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;

namespace Cyclops.MainApplication.Localization
{
    public class LocalizationManager
    {
        public static void ChangeLanguage(string culture)
        {
            Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(culture);
            Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(culture);
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
            var lang = Languages.FirstOrDefault(i =>
                string.Equals(i.Culture, Thread.CurrentThread.CurrentCulture.Name, StringComparison.InvariantCultureIgnoreCase)) ?? Languages[0];
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

    public class Language
    {
        public string Culture { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
