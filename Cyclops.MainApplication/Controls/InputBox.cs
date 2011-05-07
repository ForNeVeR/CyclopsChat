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
        }

        public static readonly DependencyProperty SendCommandProperty =
            DependencyProperty.Register("SendCommand", typeof (ICommand), typeof (InputBox),
                                        new UIPropertyMetadata(null));

        public ICommand SendCommand
        {
            get { return (ICommand) GetValue(SendCommandProperty); }
            set { SetValue(SendCommandProperty, value); }
        }

        protected override void OnPreviewKeyUp(KeyEventArgs e)
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

            
            if (SendCommand != null && SendCommand.CanExecute(null) && e.Key == Key.Enter)
                SendCommand.Execute(null);

            base.OnPreviewKeyDown(e);
        }

        
    }
}