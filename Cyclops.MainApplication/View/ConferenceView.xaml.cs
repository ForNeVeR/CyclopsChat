using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Cyclops.MainApplication.View;
using Cyclops.MainApplication.ViewModel;

namespace Cyclops.MainApplication
{
    /// <summary>
    /// Interaction logic for ConferenceView.xaml
    /// </summary>
    public partial class ConferenceView : IChatAreaView
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

        public static readonly DependencyProperty ConferenceViewModelProperty =
            DependencyProperty.Register("ConferenceViewModel", typeof(ConferenceViewModel), typeof(ConferenceView), new UIPropertyMetadata(null));

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            if (e.OriginalSource is FlowDocumentScrollViewer)
                if (e.Key == Key.LeftCtrl || Keyboard.IsKeyDown(Key.LeftCtrl))
                {
                    base.OnPreviewKeyDown(e);
                    return;
                }

            if (!(e.OriginalSource is TextBox))
                InputboxFocus();
            base.OnPreviewKeyDown(e);
        }

        private void UserControlIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if ((bool)e.NewValue)
                InputboxFocus();
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

        public void ClearOutputArea()
        {
            chatFlowDocument.Blocks.Clear();
        }

        public void OpenMenuOnHyperlink(Uri uri)
        {
        }

        public UIElement SmileElement => smilesButton;

        #endregion
    }
}