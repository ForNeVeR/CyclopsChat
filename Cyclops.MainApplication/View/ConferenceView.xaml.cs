using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cyclops.MainApplication.View;
using Cyclops.MainApplication.ViewModel;

namespace Cyclops.MainApplication
{
    /// <summary>
    /// Interaction logic for ConferenceView.xaml
    /// </summary>
    public partial class ConferenceView : UserControl
    {
        public ConferenceView()
        {
            DataContext = this;
            InitializeComponent();
        }



        public ConferenceViewModel ConferenceViewModel
        {
            get { return (ConferenceViewModel)GetValue(ConferenceViewModelProperty); }
            set { SetValue(ConferenceViewModelProperty, value); }
        }

        // Using a DependencyProperty as the backing store for ConferenceViewModel.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ConferenceViewModelProperty =
            DependencyProperty.Register("ConferenceViewModel", typeof(ConferenceViewModel), typeof(ConferenceView), new UIPropertyMetadata(null));

        
        private void SmilesButtonClick(object sender, RoutedEventArgs e)
        {
            SmilesView.OpenForChoise(smilesButton, InsertSmileAction);
        }

        private void InsertSmileAction(string mask)
        {
            if (inputBox.SelectionLength == 0)
            {
                inputBox.Text = inputBox.Text.Insert(inputBox.SelectionStart, mask);
            }
            else
            {
                inputBox.Text = inputBox.Text.Remove(0, inputBox.SelectionLength).Insert(inputBox.SelectionStart, mask);
            } 
            inputBox.SelectionStart += mask.Length;

            inputBox.Focus();
            FocusManager.SetFocusedElement(Application.Current.MainWindow, inputBox);
        }

        private void UserControl_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                FocusManager.SetFocusedElement(this, inputBox);
        }

    }
}