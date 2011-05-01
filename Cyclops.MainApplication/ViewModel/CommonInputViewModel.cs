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
        public CommonInputViewModel(String initialValue, Action<string> okAction, Func<string, bool> validator)
        {
            TextValue = string.Empty;
            Ok = new RelayCommand(() => okAction(TextValue), () => validator(TextValue));
            Cancel = new RelayCommand(CancelAction);
        }
        
        private void CancelAction()
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
