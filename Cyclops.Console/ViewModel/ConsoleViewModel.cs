using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml;
using Cyclops.Xmpp.Client;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.CommandWpf;

namespace Cyclops.Console.ViewModel;

public class ConsoleViewModel : ViewModelBase
{
    private readonly Dispatcher dispatcher;
    private readonly IXmppClient client;

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

    public ConsoleViewModel(Dispatcher dispatcher, IXmppClient client)
    {
        this.dispatcher = dispatcher;
        this.client = client;
        Send = new RelayCommand(SendCommand);
        Initialize();
    }

    private void Initialize()
    {
        client.Connected += OnConnected;
        client.ReadRawMessage += OnReadText;
        client.WriteRawMessage += OnWriteText;
        client.Error += OnError;
    }

    public override void Cleanup()
    {
        client.Connected -= OnConnected;
        client.ReadRawMessage -= OnReadText;
        client.WriteRawMessage -= OnWriteText;
        client.Error -= OnError;
    }

    private void OnConnected(object? _, object __)
    {
        InvokeAsyncIfRequired(dispatcher, () => Entries.Add("CONNECTION ESTABLISHED"));
    }

    private void OnReadText(object? _, string txt)
    {
        InvokeAsyncIfRequired(dispatcher, () => Entries.Add($"RECV: {txt}"));
    }

    private void OnWriteText(object? _, string txt)
    {
        InvokeAsyncIfRequired(dispatcher, () => Entries.Add($"SEND: {txt}"));
    }

    private void OnError(object? _, Exception ex)
    {
        InvokeAsyncIfRequired(dispatcher, () => Entries.Add($"ERROR: {ex.Message}"));
    }

    private void SendCommand()
    {
        try
        {
            var document = new XmlDocument();
            document.LoadXml(XmlToSend);

            var element = document.DocumentElement;
            if (element == null) return;

            client.SendElement(element);
            XmlToSend = "";
        }
        catch (XmlException)
        {
            // ignore
        }
    }

    private static void InvokeAsyncIfRequired(Dispatcher dispatcher, Action action)
    {
        if (dispatcher.CheckAccess())
            action();
        else
            dispatcher.InvokeAsync(action);
    }
}
