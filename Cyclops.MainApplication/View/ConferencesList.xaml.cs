using Cyclops.MainApplication.ViewModel;
using GalaSoft.MvvmLight;

namespace Cyclops.MainApplication.View
{
    /// <summary>
    /// Interaction logic for ConferencesList.xaml
    /// </summary>
    public partial class ConferencesList
    {
        public ConferencesList()
        {
            if (ViewModelBase.IsInDesignModeStatic)
            {
                DataContext = new ConferenceViewModelDesignTime();
                InitializeComponent();
                return;
            }
            InitializeComponent();
            ((ConferencesListViewModel) DataContext).Close += (s, e) => DialogResult = true;
        }
    }
}