﻿using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Cyclops.MainApplication.ViewModel;

namespace Cyclops.MainApplication.View
{
    /// <summary>
    /// Interaction logic for PrivateView.xaml
    /// </summary>
    public partial class PrivateView : IChatAreaView
    {
        public static readonly DependencyProperty PrivateViewModelProperty =
            DependencyProperty.Register("PrivateViewModel", typeof (PrivateViewModel), typeof (PrivateView),
                                        new UIPropertyMetadata(null));

        public PrivateView()
        {
            InitializeComponent();
            DataContext = this;
        }

        public PrivateViewModel PrivateViewModel
        {
            get { return (PrivateViewModel) GetValue(PrivateViewModelProperty); }
            set { SetValue(PrivateViewModelProperty, value); }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (!(e.OriginalSource is IInputElement))
                InputboxFocus();
            base.OnPreviewKeyDown(e);
        }
        
        #region Implementation of IChatAreaView

        public void InputboxFocus()
        {
            Keyboard.Focus(inputBox);
        }

        public int InputBoxSelectionLength
        {
            get { return inputBox.SelectionLength; }
            set { inputBox.SelectionLength = value; }
        }

        public int InputBoxSelectionStart
        {
            get { return inputBox.SelectionStart; }
            set { inputBox.SelectionStart = value; }
        } 

        #endregion
    }
}