using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Cyclops.MainApplication.Localization;

/// <summary>
/// This wrapper allows to change language directly in runtime (including XAML binding)
/// </summary>
public class ResourceWrapper : INotifyPropertyChanged
    //EX: Text="{Binding Path=ApplicationStrings.ApplicationName, Source={StaticResource ResourceWrapper}}"
{

    private Conference conference = new();
    public Conference Conference
    {
        get => conference;
        set
        {
            conference = value;
            OnPropertyChanged();
        }
    }

    private Login login = new();
    public Login Login
    {
        get => login;
        set
        {
            login = value;
            OnPropertyChanged();
        }
    }

    private ContextMenus contextMenu = new();
    public ContextMenus ContextMenus
    {
        get => contextMenu;
        set
        {
            contextMenu = value;
            OnPropertyChanged();
        }
    }

    private Common common = new();
    public Common Common
    {
        get => common;
        set
        {
            common = value;
            OnPropertyChanged();
        }
    }

    private Vcard vcard = new();
    public Vcard Vcard
    {
        get => vcard;
        set
        {
            vcard = value;
            OnPropertyChanged();
        }
    }

    private ConferenceList conferenceList = new();
    public ConferenceList ConferenceList
    {
        get => conferenceList;
        set
        {
            conferenceList = value;
            OnPropertyChanged();
        }
    }

    private Main main = new();
    public Main Main
    {
        get => main;
        set
        {
            main = value;
            OnPropertyChanged();
        }
    }

    private Settings settings = new();
    public Settings Settings
    {
        get => settings;
        set
        {
            settings = value;
            OnPropertyChanged();
        }
    }

    public void Refresh()
    {
        Login = new Login();
        Conference = new Conference();
        Main = new Main();
        ConferenceList = new ConferenceList();
        Common = new Common();
        Vcard = new Vcard();
        Settings = new Settings();
        ContextMenus = new ContextMenus();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    protected void OnPropertyChanged([CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
