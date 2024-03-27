using Cyclops.MainApplication.ViewModel;
using static Cyclops.Wpf.DesignerUtil;

namespace Cyclops.MainApplication.View;

/// <summary>
/// Interaction logic for ConferencesList.xaml
/// </summary>
public partial class ConferencesList
{
    public ConferencesList()
    {
        if (IsInDesignMode)
        {
            DataContext = new ConferenceViewModelDesignTime();
            InitializeComponent();
            return;
        }
        InitializeComponent();
        ((ConferencesListViewModel) DataContext).Close += (s, e) => DialogResult = true;
    }

    protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
    {
        if (e.Key == System.Windows.Input.Key.Escape)
            Close();
        base.OnKeyDown(e);
    }
}
