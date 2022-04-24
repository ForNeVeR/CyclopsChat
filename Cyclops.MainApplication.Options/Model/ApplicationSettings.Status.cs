namespace Cyclops.MainApplication.Options.Model;

partial class ApplicationSettings
{
    private int autoAwayAfter;
    private int autoExtendedAwayAfter;
    private bool showStatusChangingMessages;
        
    public int AutoAwayAfter
    {
        get { return autoAwayAfter; }
        set
        {
            autoAwayAfter = value;
            RaisePropertyChanged("AutoAwayAfter");
        }
    }

    public int AutoExtendedAwayAfter
    {
        get { return autoExtendedAwayAfter; }
        set
        {
            autoExtendedAwayAfter = value;
            RaisePropertyChanged("AutoExtendedAwayAfter");
        }
    }

    public bool ShowStatusChangingMessages
    {
        get { return showStatusChangingMessages; }
        set
        {
            showStatusChangingMessages = value;
            RaisePropertyChanged("ShowStatusChangingMessages");
        }
    }

    private void CloneStatusProperties(ApplicationSettings cloneObj)
    {
        cloneObj.AutoAwayAfter = AutoAwayAfter;
        cloneObj.AutoExtendedAwayAfter = AutoExtendedAwayAfter;
        cloneObj.ShowStatusChangingMessages = ShowStatusChangingMessages;
    }

    private void SetStatusDefaultValues()
    {
        AutoAwayAfter = 15;
        AutoExtendedAwayAfter = 60;
    }
}