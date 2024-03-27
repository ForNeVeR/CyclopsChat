using Cyclops.MainApplication.ViewModel;
using static Cyclops.Wpf.DesignerUtil;

namespace Cyclops.MainApplication.View.Popups;

public class ErrorNotificationViewModel : ViewModelBaseEx
{
    public ErrorNotificationViewModel()
    {
        if (IsInDesignMode)
        {
            Title = "Common error";
            Body = "You are not allowed to perform an action at this conference.";
        }
    }

    private string title;
    public string Title
    {
        get { return title; }
        set
        {
            title = value;
            OnPropertyChanged();
        }
    }

    private string body;
    public string Body
    {
        get { return body; }
        set
        {
            body = value;
            OnPropertyChanged();
        }
    }
}
