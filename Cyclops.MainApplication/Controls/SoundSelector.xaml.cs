using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;

namespace Cyclops.MainApplication.Controls
{
    /// <summary>
    /// Interaction logic for SoundSelector.xaml
    /// </summary>
    public partial class SoundSelector
    {
        public SoundSelector()
        {
            Browse = new RelayCommand(BrowseAction);
            PlaySound = new RelayCommand(PlaySoundAction, () => !string.IsNullOrEmpty(Text));
            Clear = new RelayCommand(ClearAction, () => !string.IsNullOrEmpty(Text));

            InitializeComponent();
        }

        private void ClearAction()
        {
            SoundPlayer.Stop();
            Text = string.Empty;
        }

        private void BrowseAction()
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.InitialDirectory = System.IO.Path.Combine(
// ReSharper disable AssignNullToNotNullAttribute
                System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), 
// ReSharper restore AssignNullToNotNullAttribute
                @"Data\Sounds");

            dlg.Filter = "Sounds(*.WAV;*.WMA;*.MP3;)|*.WAV;*.WMA;*.MP3;|All files (*.*)|*.*";

            if (dlg.ShowDialog() == true)
            {
                ClearAction();
                Text = dlg.FileName;
                PlaySoundAction();
            }

        }

        private void PlaySoundAction()
        {
            if (!File.Exists(Text))
            {
                MessageBox.Show("Sound file was not found");
                return;
            }

            try
            {
                SoundPlayer.Play(Text, true);
            }
            catch(Exception exc)
            {
                MessageBox.Show("Invalid sound format. " + exc.Message);
            }
        }

        public RelayCommand Browse { get; set; }
        public RelayCommand PlaySound { get; set; }
        public RelayCommand Clear { get; set; }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(SoundSelector), new UIPropertyMetadata(""));
    }
}
