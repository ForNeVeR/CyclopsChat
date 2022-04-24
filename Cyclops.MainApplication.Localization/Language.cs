namespace Cyclops.MainApplication.Localization;

public class Language
{
    public string Culture { get; }
    public string Name { get; }

    public override string ToString()
    {
        return Name;
    }

    public Language(string culture, string name)
    {
        Culture = culture;
        Name = name;
    }
}
