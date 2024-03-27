using Cyclops.Console.ViewModel;
using Cyclops.Core;
using Cyclops.Core.Resource;

namespace Cyclops.Console;

public partial class ConsoleWindow : IDebugWindow
{
    private new ConsoleViewModel? DataContext
    {
        get => (ConsoleViewModel)base.DataContext;
        set => base.DataContext = value;
    }

    public ConsoleWindow()
    {
        InitializeComponent();
    }

    public void ShowConsole(IUserSession session)
    {
        if (DataContext is not null)
            DataContext.IsActive = false;

        var client = ((UserSession)session).XmppClient;
        DataContext = new ConsoleViewModel(Dispatcher, client);
        Show();
    }
}
