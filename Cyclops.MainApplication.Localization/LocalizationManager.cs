using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows;

namespace Cyclops.MainApplication.Localization;

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
        if (Application.Current.Resources["ResourceWrapper"] is ResourceWrapper rw)
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

    public Language[] Languages { get; }

    private Language? selectedLanguage;
    public Language? SelectedLanguage
    {
        get => selectedLanguage;
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
            new Language("en-US", "English"),
            new Language("ru-RU", "Russian")
        };
        var lang = Languages.First();
        SelectedLanguage = lang;
    }

    private static LocalizationManager? instance;
    public static LocalizationManager Instance => instance ??= new LocalizationManager();
}
