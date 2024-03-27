using System.Globalization;
using System.Linq;
using System.Xml.Serialization;
using Cyclops.MainApplication.Localization;

namespace Cyclops.MainApplication.Options.Model;

partial class ApplicationSettings
{
    private bool blinkOnlyOnPrivates;
    private bool disableBlinking;
    private bool enableErrorPopups;
    private bool enablePrivatePopups;
    private bool isSmilesAnimated;
    private int popupStaysOpenning;
    private string? selectedLanguage;
    private bool showEntryAndExits;
    private bool showRoleChanges;
    private int smilesLimitInMessage;

    [XmlIgnore]
    public string[] Languages => LocalizationManager.Instance.Languages.Select(i => i.Culture).ToArray();

    public string? SelectedLanguage
    {
        get => selectedLanguage;
        set
        {
            selectedLanguage = value;
            OnPropertyChanged();
        }
    }

    public int SmilesLimitInMessage
    {
        get => smilesLimitInMessage;
        set
        {
            smilesLimitInMessage = value;
            OnPropertyChanged();
        }
    }

    public bool EnablePrivatePopups
    {
        get => enablePrivatePopups;
        set
        {
            enablePrivatePopups = value;
            OnPropertyChanged();
        }
    }

    public bool EnableErrorPopups
    {
        get => enableErrorPopups;
        set
        {
            enableErrorPopups = value;
            OnPropertyChanged();
        }
    }

    public int PopupStaysOpenning
    {
        get => popupStaysOpenning;
        set
        {
            popupStaysOpenning = value;
            OnPropertyChanged();
        }
    }

    public bool DisableBlinking
    {
        get => disableBlinking;
        set
        {
            disableBlinking = value;
            OnPropertyChanged();
        }
    }

    public bool ShowEntryAndExits
    {
        get => showEntryAndExits;
        set
        {
            showEntryAndExits = value;
            OnPropertyChanged();
        }
    }

    public bool ShowRoleChanges
    {
        get => showRoleChanges;
        set
        {
            showRoleChanges = value;
            OnPropertyChanged();
        }
    }

    public bool BlinkOnlyOnPrivates
    {
        get => blinkOnlyOnPrivates;
        set
        {
            blinkOnlyOnPrivates = value;
            OnPropertyChanged();
        }
    }

    public bool IsSmilesAnimated
    {
        get => isSmilesAnimated;
        set
        {
            isSmilesAnimated = value;
            OnPropertyChanged();
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
        cloneObj.ShowEntryAndExits = ShowEntryAndExits;
        cloneObj.ShowRoleChanges = ShowRoleChanges;
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
        ShowRoleChanges = true;
        ShowEntryAndExits = true;
    }
}
