using Cyclops.MainApplication.MessageDecoration;
using Cyclops.MainApplication.MessageDecoration.Decorators;
using Cyclops.MainApplication.View;
using Cyclops.Xmpp.Protocol;
using GalaSoft.MvvmLight.CommandWpf;

namespace Cyclops.MainApplication.ViewModel;

public abstract class ChatAreaViewModel : ViewModelBaseEx
{
    private int unreadMessagesCount;
    private string currentlyTypedMessage;
    private bool isActive;

    public virtual bool IsPrivate => false;

    public virtual bool IsConference => false;

    public RelayCommand Close { get; set; }
    public IChatAreaView View { get; set; }
    public RelayCommand ClearOutputArea { get; private set; }
    public RelayCommand ShowSettings { get; private set; }
    public RelayCommand SendMessage { get; private set; }
    public RelayCommand InsertSmiles { get; private set; }

    protected ChatAreaViewModel(IChatAreaView view)
    {
        View = view;
        ClearOutputArea = new RelayCommand(() => View.ClearOutputArea());
        Close = new RelayCommand(CloseAction);
        ShowSettings = new RelayCommand(() => ApplicationContext.Current.MainViewModel.ShowSettings());
        SendMessage = new RelayCommand(OnSendMessage, OnSendMessageCanExecute);
        InsertSmiles = new RelayCommand(InsertSmilesAction);
        DecoratorsRegistry.NickClick += DecoratorsRegistryNickClick;
    }

    private void InsertSmilesAction()
    {
        SmilesView.OpenForChoise(View.SmileElement, InsertSmileIntoInputAction);
    }

    private void InsertSmileIntoInputAction(string mask)
    {
        if (CurrentlyTypedMessage == null)
            CurrentlyTypedMessage = string.Empty;

        if (View.InputBoxSelectionLength == 0)
            CurrentlyTypedMessage = CurrentlyTypedMessage.Insert(View.InputBoxSelectionStart, mask);
        else
            CurrentlyTypedMessage = CurrentlyTypedMessage.Remove(0, View.InputBoxSelectionLength).Insert(View.InputBoxSelectionStart, mask);

        View.InputBoxSelectionStart += mask.Length;
        View.InputboxFocus();
    }

    protected virtual void CloseAction()
    {
    }

    protected virtual void OnSendMessage()
    {
    }

    protected virtual bool OnSendMessageCanExecute()
    {
        return !string.IsNullOrEmpty(CurrentlyTypedMessage);
    }

    private void DecoratorsRegistryNickClick(object sender, NickEventArgs e)
    {
        if (!IsActive || string.IsNullOrEmpty(e.Nick))
            return;

        AppendText(e.Nick + ": ");
        if (e.Id != null)
            OnNickClick(e.Id);
    }

    protected virtual void OnNickClick(Jid id)
    {
    }

    protected string RemoveEndNewLineSymbol(string message)
    {
        if (string.IsNullOrEmpty(message))
            return string.Empty;
        if (message.EndsWith("\r\n"))
            return message.Remove(message.Length - 2);
        return message;
    }

    protected void AppendText(string nick)
    {
        if (CurrentlyTypedMessage == null)
            CurrentlyTypedMessage = string.Empty;

        if (View.InputBoxSelectionLength == 0)
            CurrentlyTypedMessage = CurrentlyTypedMessage.Insert(View.InputBoxSelectionStart, nick);
        else
            CurrentlyTypedMessage = CurrentlyTypedMessage.Remove(0, View.InputBoxSelectionLength).Insert(View.InputBoxSelectionStart, nick);
        View.InputBoxSelectionLength = 0;
        View.InputBoxSelectionStart = CurrentlyTypedMessage.Length;
        View.InputboxFocus();
    }

    public bool IsActive
    {
        get { return isActive; }
        set
        {
            isActive = value;
            RaisePropertyChanged("IsActive");

            if (value)
                UnreadMessagesCount = 0;
        }
    }

    public int UnreadMessagesCount
    {
        get { return unreadMessagesCount; }
        set
        {
            unreadMessagesCount = value;
            RaisePropertyChanged("UnreadMessagesCount");
        }
    }

    public string CurrentlyTypedMessage
    {
        get { return currentlyTypedMessage; }
        set
        {
            currentlyTypedMessage = value;
            RaisePropertyChanged("CurrentlyTypedMessage");
        }
    }
}
