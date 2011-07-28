namespace Cyclops.MainApplication.Localization
{
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