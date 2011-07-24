using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Cyclops.MainApplication.Controls
{
    /// <summary>
    /// Text box with command on 'enter' button
    /// </summary>
    public class InputBox : TextBox
    {
        public InputBox()
        {
            this.TextWrapping = TextWrapping.Wrap;
            AcceptsReturn = true;
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
        }

        public static readonly DependencyProperty SendCommandProperty =
            DependencyProperty.Register("SendCommand", typeof (ICommand), typeof (InputBox),
                                        new UIPropertyMetadata(null));

        public ICommand SendCommand
        {
            get { return (ICommand) GetValue(SendCommandProperty); }
            set { SetValue(SendCommandProperty, value); }
        }

        protected override void OnPreviewKeyDown(KeyEventArgs e)
        {
            //Ctrl + Enter handle
            if (e.Key == Key.Enter && Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                var newline = "\r\n";
                if (SelectionLength == 0)
                    Text = Text.Insert(SelectionStart, newline);
                else
                    Text = Text.Remove(0, SelectionLength).Insert(SelectionStart, newline);
                SelectedText = "";
                SelectionStart = Text.Length;
                e.Handled = true;
                return;
            }

            bool enterPressed = e.Key == Key.Return || e.Key == Key.Enter;

            if (SendCommand != null && SendCommand.CanExecute(null) && enterPressed)
            {
                SendCommand.Execute(null);
                e.Handled = true;
            }
            else if (enterPressed)
                e.Handled = true;

            base.OnPreviewKeyDown(e);
        }

        
    }
}