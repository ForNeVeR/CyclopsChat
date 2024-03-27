using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using Cyclops.Core;
using Cyclops.Core.CustomEventArgs;
using Cyclops.MainApplication.View.Dialogs;
using Cyclops.Xmpp.Protocol;
using GalaSoft.MvvmLight.CommandWpf;

namespace Cyclops.MainApplication.ViewModel;

public class ConferencesListViewModel : ViewModelBaseEx
{
    private IEnumerable<ConferenceInfo> conferences;
    private string filter;
    private bool isBusy;
    private string openWithNick;
    private ConferenceInfo selectedConference;
    private IEnumerable<ConferenceInfo> sourceConferences;
    private ConferencesServiceItem selectedService;
    private IEnumerable<ConferencesServiceItem> conferenceServices;

    public ConferencesListViewModel()
    {
        OpenConference = new RelayCommand(() => OpenConferenceAction(SelectedConference.Id), OpenConferenceCanExecute);
        CreateNewConference = new RelayCommand(CreateNewConferenceAction, CreateNewConferenceCanExecute);
        Session = ChatObjectFactory.GetSession();
        Session.ConferencesListReceived += ConferencesListReceived;


        if (IsInDesignMode)
        {
            Conferences = new[]
            {
                new ConferenceInfo {Id = new Jid("cyclops", ""), Name = "Cyclops development test", IsOpened = true},
                new ConferenceInfo {Id = new Jid("main", ""), Name = "Main (5)", IsOpened = true},
                new ConferenceInfo {Id = new Jid("anime", ""), Name = "Anime"},
            };
            return;
        }


        ConferencesServiceItem currentService = new ConferencesServiceItem
        {
            ConferenceService = null,
            DisplayName = Localization.ConferenceList.CurrentRoomsServer
        };

        var services = ApplicationContext.Current.CurrentProfile.FriendlyConferencesServices.ToList();
        services.Add(currentService);
        ConferenceServices = services;
        SelectedService = currentService;

        OpenWithNick = Session.CurrentUserId.Local;
    }

    public IUserSession Session { get; private set; }

    private bool CreateNewConferenceCanExecute()
    {
        return Session.IsAuthenticated && Session.ConferenceServiceId != null;
    }

    private void CreateNewConferenceAction()
    {
        DialogManager.ShowStringInputDialog(Localization.ConferenceList.CreateNew,
            string.Empty, CreateConferenceSubmit, CreateConferenceValidator);
    }

    private void CreateConferenceSubmit(string arg)
    {
        Jid? id = null;
        try
        {
            if (arg.Contains("@"))
                id = Jid.Parse(arg);
            else
                id = new Jid(arg, Session.ConferenceServiceId.Domain, OpenWithNick);
            OpenConferenceAction(id);
        }
        catch(Exception exc)
        {
            MessageBox.Show("Can't create conference (wrong name)", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }

    private static bool CreateConferenceValidator(string arg)
    {
        return !string.IsNullOrWhiteSpace(arg)
               && arg.Length < 100
               && arg.Count(i => i == '@') < 2
               && arg.Count(i => i == '/') < 2;
    }

    public bool IsBusy
    {
        get { return isBusy; }
        set
        {
            isBusy = value;
            RaisePropertyChanged("IsBusy");
        }
    }

    public IEnumerable<ConferenceInfo> Conferences
    {
        get { return conferences; }
        set
        {
            conferences = value;
            RaisePropertyChanged("Conferences");
        }
    }

    public IEnumerable<ConferencesServiceItem> ConferenceServices
    {
        get { return conferenceServices; }
        set
        {
            conferenceServices = value;
            RaisePropertyChanged("Conferences");
        }
    }

    public ConferencesServiceItem SelectedService
    {
        get { return selectedService; }
        set
        {
            selectedService = value;
            RaisePropertyChanged("Conferences");

            IsBusy = true;
            if (selectedService.ConferenceService != null && selectedService.ConferenceService.Equals(Namespaces.Bookmarks))
            {
                Session.RaiseBookmarksReceived();
            }
            else
            {
                Session.RefreshConferenceList(value.ConferenceService);
            }

        }
    }

    public ConferenceInfo SelectedConference
    {
        get { return selectedConference; }
        set
        {
            selectedConference = value;
            RaisePropertyChanged("SelectedConference");
        }
    }

    public string Filter
    {
        get { return filter; }
        set
        {
            filter = value;
            RaisePropertyChanged("Filter");
            if (string.IsNullOrEmpty(value))
                Conferences = sourceConferences.ToArray();
            else
                Conferences = sourceConferences.Where(i =>
                    /*i.Name.ToLower().Contains(value.ToLower()) ||*/
                    i.Id.Local.ToLower().Contains(value.ToLower())).ToArray();
        }
    }

    public string OpenWithNick
    {
        get { return openWithNick; }
        set
        {
            openWithNick = value;
            RaisePropertyChanged("OpenWithNick");
        }
    }

    public RelayCommand OpenConference { get; set; }
    public RelayCommand CreateNewConference { get; set; }

    private void ConferencesListReceived(object sender, ConferencesListEventArgs e)
    {
        IsBusy = false;
        if (!e.Success)
            return;

        var conferenceIds = ChatObjectFactory.GetSession().Conferences.Select(i => i.ConferenceId);

        sourceConferences = Conferences = e.Result.Select(i =>
            new ConferenceInfo
            {
                Id = i.Item1,
                Name = i.Item2,
                IsOpened = conferenceIds.Any(c => i.Item1.BaresEqual(c))
            }).OrderBy(i => i.Id.Local).ToList();
    }

    private bool OpenConferenceCanExecute()
    {
        return SelectedConference != null;
    }

    private void OpenConferenceAction(Jid? id)
    {
        if (id == null)
            return;

        var jid = id.Value;
        IUserSession session = ChatObjectFactory.GetSession();

        IConference existsConference = session.Conferences.FirstOrDefault(i => i.ConferenceId.Bare == jid.Bare);
        if (existsConference != null)
        {
            if (existsConference.IsInConference)
            {
                MessageBox.Show(Localization.ConferenceList.AlreadyInRoom, "Warrning", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            existsConference.LeaveAndClose();
        }

        string nick = string.IsNullOrWhiteSpace(OpenWithNick) ? session.CurrentUserId.Local : OpenWithNick;
        session.OpenConference(jid.WithResource(nick));
        Close(this, EventArgs.Empty);
    }

    public event EventHandler Close = delegate { };
}

public class ConferenceInfo : NotifyPropertyChangedBase
{
    private Jid id;
    public Jid Id
    {
        get { return id; }
        set
        {
            id = value;
            OnPropertyChanged("Id");
        }
    }

    private string name;
    public string Name
    {
        get { return name; }
        set
        {
            name = value;
            OnPropertyChanged("Name");
        }
    }

    public bool IsOpened { get; set; }
}
