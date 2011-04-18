using Cyclops.MainApplication.ViewModel;

namespace Cyclops.MainApplication.View
{
    /// <summary>
    /// Interaction logic for ConferencesList.xaml
    /// </summary>
    public partial class ConferencesList
    {
        public ConferencesList()
        {
            InitializeComponent();
            ((ConferencesListViewModel) DataContext).Close += (s, e) => DialogResult = true;
        }
    }
}