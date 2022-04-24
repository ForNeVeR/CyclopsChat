using System;
using Cyclops.MainApplication.Options.Model;
using GalaSoft.MvvmLight.Command;

namespace Cyclops.MainApplication.Options.ViewModel;

public class SettingsViewModel : ApplicationSettings
{
    private readonly Action? acceptAction;
    private readonly Action? cancelAction;

    public SettingsViewModel(Action acceptAction, Action cancelAction)
    {
        if (IsInDesignMode)
            return;

        this.acceptAction = acceptAction;
        this.cancelAction = cancelAction;

        Commit = new RelayCommand(CommitAction);
        Cancel = new RelayCommand(CancelAction);
    }

    private void CancelAction()
    {
        cancelAction!();
    }

    private void CommitAction()
    {
        acceptAction!();
    }

    public RelayCommand? Commit { get; }
    public RelayCommand? Cancel { get; }

    private ApplicationSettings? settings;
    public ApplicationSettings? Settings
    {
        get => settings;
        set
        {
            settings = value;
            RaisePropertyChanged("Settings");
        }
    }
}
