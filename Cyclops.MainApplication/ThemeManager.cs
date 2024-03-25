using System;
using System.IO;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;

namespace Cyclops.MainApplication
{
    public static class ThemeManager
    {
        public static void ApplyDefault()
        {
            var dictionaries = Application.Current.Resources.MergedDictionaries;
            
            //i don't want to remove "fixed" RD, but how can i determinate it in the code??
            for (int i = 2; i < dictionaries.Count; i++)
                dictionaries.Remove(dictionaries[i]); 

            dictionaries.Add(LoadDictionary(@"Data\Themes\Default\OutputAreaStyles.xaml"));
            dictionaries.Add(LoadDictionary(@"Data\Themes\Default\General.xaml"));
            dictionaries.Add(LoadDictionary(@"Data\Themes\Default\SettingsViewStyles.xaml"));
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
//#if DEBUG
                MessageBox.Show("Parse error: " + exc.Message + "; in file: " + file);
//#endif
                ChatObjectFactory.GetDebugLogger().LogError("Error while loading resource dictionary " + file, exc);
                return new ResourceDictionary();
            }
        }
    }
}