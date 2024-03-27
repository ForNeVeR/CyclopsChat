using CommunityToolkit.Mvvm.ComponentModel;
using Cyclops.MainApplication.Options.Model;

namespace Cyclops.MainApplication.ViewModel;

public class ViewModelBaseEx : ObservableRecipient
{
    public ApplicationSettings Settings => ApplicationContext.Current.ApplicationSettings;

    public MainViewModel Main => ApplicationContext.Current.MainViewModel;

    //private bool isBusy;
    //public bool IsBusy
    //{
    //    get { return isBusy; }
    //    set
    //    {
    //        isBusy = value;
    //        RaisePropertyChanged("IsBusy");
    //    }
    //}
}
