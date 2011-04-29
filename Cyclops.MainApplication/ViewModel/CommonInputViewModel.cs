using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.ViewModel
{
    public class CommonInputViewModel : ViewModelBase
    {
        public CommonInputViewModel()
        {
            Ok = new RelayCommand(OkAction, OkCanExecute);
            Cancel = new RelayCommand(CancelAction, CancelCanExecute);
        }

        private bool CancelCanExecute()
        {
            return true;
        }

        private void CancelAction()
        {
        }

        private bool OkCanExecute()
        {
            return true;
        }

        private void OkAction()
        {
        }

        public RelayCommand Ok { get; set; }
        public RelayCommand Cancel { get; set; }

        private string textValue;
        public string TextValue
        {
            get { return textValue; }
            set
            {
                textValue = value;
                RaisePropertyChanged("TextValue");
            }
        }
    }
}
