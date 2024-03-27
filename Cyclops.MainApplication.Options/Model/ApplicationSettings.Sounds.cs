namespace Cyclops.MainApplication.Options.Model;

partial class ApplicationSettings
{
    private bool disableAllSounds;
    private bool soundEvenIfActive;
    private string? soundOnIncomingPrivate;
    private string? soundOnIncomingPublic;
    private string? soundOnStatusChange;
    private string? soundOnSystemMessage;
    private string? soundOnUserJoin;
    private string? soundOnUserLeave;

    public string? SoundOnUserLeave
    {
        get => soundOnUserLeave;
        set
        {
            soundOnUserLeave = value;
            OnPropertyChanged();
        }
    }

    public string? SoundOnUserJoin
    {
        get => soundOnUserJoin;
        set
        {
            soundOnUserJoin = value;
            OnPropertyChanged();
        }
    }

    public string? SoundOnIncomingPrivate
    {
        get => soundOnIncomingPrivate;
        set
        {
            soundOnIncomingPrivate = value;
            OnPropertyChanged();
        }
    }

    public string? SoundOnIncomingPublic
    {
        get => soundOnIncomingPublic;
        set
        {
            soundOnIncomingPublic = value;
            OnPropertyChanged();
        }
    }

    public string? SoundOnSystemMessage
    {
        get => soundOnSystemMessage;
        set
        {
            soundOnSystemMessage = value;
            OnPropertyChanged();
        }
    }

    public string? SoundOnStatusChange
    {
        get => soundOnStatusChange;
        set
        {
            soundOnStatusChange = value;
            OnPropertyChanged();
        }
    }

    public bool DisableAllSounds
    {
        get => disableAllSounds;
        set
        {
            disableAllSounds = value;
            OnPropertyChanged();
        }
    }

    public bool SoundEvenIfActive
    {
        get => soundEvenIfActive;
        set
        {
            soundEvenIfActive = value;
            OnPropertyChanged();
        }
    }

    private void CloneSoundsProperties(ApplicationSettings cloneObj)
    {
        cloneObj.SoundOnUserLeave = SoundOnUserLeave;
        cloneObj.SoundOnUserJoin = SoundOnUserJoin;
        cloneObj.SoundOnIncomingPrivate = SoundOnIncomingPrivate;
        cloneObj.SoundOnIncomingPublic = SoundOnIncomingPublic;
        cloneObj.SoundOnSystemMessage = SoundOnSystemMessage;
        cloneObj.SoundOnStatusChange = SoundOnStatusChange;
        cloneObj.DisableAllSounds = DisableAllSounds;
        cloneObj.SoundEvenIfActive = SoundEvenIfActive;
    }

    private void SetSoundsDefaultValues()
    {
    }
}
