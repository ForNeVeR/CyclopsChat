using System;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight.CommandWpf;

namespace Cyclops.MainApplication.View.Dialogs
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog
    {
        private readonly string initialValue;

        protected InputDialog(string initialValue, Action<string> okAction, Func<string, bool> validator)
        {
            TextValue = this.initialValue = initialValue;
            Ok = new RelayCommand(() =>
                                      {
                                          okAction(TextValue);
                                          DialogResult = true;
                                      }, () => initialValue != TextValue && validator(TextValue));
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
