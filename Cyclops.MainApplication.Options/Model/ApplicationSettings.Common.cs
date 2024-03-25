namespace Cyclops.MainApplication.Options.Model;

partial class ApplicationSettings
{
    private bool hideOnWindowClosing;
    private bool startWithWindows;

    public bool StartWithWindows
    {
        get { return startWithWindows; }
        set
        {
            startWithWindows = value;
            RaisePropertyChanged("StartWithWindows");
        }
    }

    public bool HideOnWindowClosing
    {
        get { return hideOnWindowClosing; }
        set
        {
            hideOnWindowClosing = value;
            RaisePropertyChanged("HideOnWindowClosing");
        }
    }

    private void CloneCommonProperties(ApplicationSettings cloneObj)
    {
        cloneObj.StartWithWindows = StartWithWindows;
        cloneObj.HideOnWindowClosing = HideOnWindowClosing;
    }

    private void SetCommonDefaultValues()
    {
        StartWithWindows = false;
        HideOnWindowClosing = true;
    }
}