using Cyclops.Console.ViewModel;
using Cyclops.Core;

namespace Cyclops.Console
{
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
            DataContext?.Cleanup();
            DataContext = new ConsoleViewModel(session.XmppClient);
            Show();
        }
    }
}

