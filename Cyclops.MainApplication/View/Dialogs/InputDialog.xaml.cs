using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.View.Dialogs
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog
    {
        protected InputDialog(string initialValue, Action<string> okAction, Func<string, bool> validator)
        {
            TextValue = initialValue;
            Ok = new RelayCommand(() =>
                                      {
                                          okAction(TextValue);
                                          DialogResult = true;
                                      }, () => validator(TextValue));
            Cancel = new RelayCommand(CancelAction);
            DataContext = this;
            InitializeComponent();
        }

        private void CancelAction()
        {
            DialogResult = false;
        }

        public static bool ShowForEdit(string title, string initialValue, Action<string> okAction, Func<string, bool> validator)
        {
            var dlg = new InputDialog(initialValue, okAction, validator);
            dlg.Owner = Application.Current.MainWindow;
            dlg.Title = title;
            return dlg.ShowDialog() == true;
        }

        public RelayCommand Ok { get; set; }
        public RelayCommand Cancel { get; set; }

        public string TextValue
        {
            get { return (string)GetValue(TextValueProperty); }
            set { SetValue(TextValueProperty, value); }
        }

        public static readonly DependencyProperty TextValueProperty =
            DependencyProperty.Register("TextValue", typeof(string), typeof(InputDialog), new UIPropertyMetadata(""));

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            FocusManager.SetFocusedElement(this, textBox);
        }
    }
}
