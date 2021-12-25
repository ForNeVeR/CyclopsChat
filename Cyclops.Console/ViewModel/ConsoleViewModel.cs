using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Xml;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using jabber.client;
using jabber.connection;

namespace Cyclops.Console.ViewModel
{
    public class ConsoleViewModel : ViewModelBase
    {
        private readonly JabberClient client;

        public ObservableCollection<string> Entries { get; } = new();

        private string xmlToSend = "";
        public string XmlToSend
        {
            get => xmlToSend;
            set
            {
                if (value == xmlToSend) return;
                xmlToSend = value;
                RaisePropertyChanged(nameof(XmlToSend));
            }
        }

        public ICommand Send { get; }

        public ConsoleViewModel(JabberClient client)
        {
            this.client = client;
            Send = new RelayCommand(SendCommand);
            Initialize();
        }

        private void Initialize()
        {
            client.OnConnect += OnConnect;
            client.OnReadText += OnReadText;
            client.OnWriteText += OnWriteText;
            client.OnError += OnError;
        }

        public override void Cleanup()
        {
            client.OnConnect -= OnConnect;
            client.OnReadText -= OnReadText;
            client.OnWriteText -= OnWriteText;
            client.OnError -= OnError;
        }

        private void OnConnect(object sender, StanzaStream stream)
        {
            Entries.Add("CONNECTION ESTABLISHED");
        }

        private void OnReadText(object sender, string txt)
        {
            Entries.Add($"RECV: {txt}");
        }

        private void OnWriteText(object sender, string txt)
        {
            Entries.Add($"SEND: {txt}");
        }

        private void OnError(object sender, Exception ex)
        {
            Entries.Add($"ERROR: {ex.Message}");
        }

        private void SendCommand()
        {
            try
            {
                var document = new XmlDocument();
                document.LoadXml(XmlToSend);

                client.Write(document.DocumentElement);

                XmlToSend = "";
            }
            catch (XmlException)
            {
                // ignore
            }
        }
    }
}
