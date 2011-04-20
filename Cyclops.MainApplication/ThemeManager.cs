using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;

namespace Cyclops.MainApplication
{
    public static class ThemeManager
    {
        public static void ApplyDefault()
        {
            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(LoadDictionary(@"Data\Themes\Default\OutputAreaStyles.xaml"));
            //Application.Current.Resources.MergedDictionaries.Add(LoadDictionary(@"Themes\Default\Theme.xaml"));
        }

        private static ResourceDictionary LoadDictionary(string file)
        {
            try
            {
                using (var fs = new FileStream(file, FileMode.Open))
                    return XamlReader.Load(fs) as ResourceDictionary;
            }
            catch(Exception exc)
            {
                ChatObjectFactory.GetLogger().LogError("Error while loading resource dictionary " + file, exc);
                return new ResourceDictionary();
            }
        }
    }
}