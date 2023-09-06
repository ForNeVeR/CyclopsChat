using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cyclops.MainApplication.Configuration;
using Cyclops.MainApplication.Options.Model;
using GalaSoft.MvvmLight;

namespace Cyclops.MainApplication.ViewModel
{
    public class ViewModelBaseEx : ViewModelBase
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
}
