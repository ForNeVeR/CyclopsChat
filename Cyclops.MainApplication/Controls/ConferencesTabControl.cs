using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Cyclops.MainApplication.Options.View;
using Cyclops.MainApplication.View;
using Cyclops.MainApplication.ViewModel;

namespace Cyclops.MainApplication.Controls
{
    public class ConferencesTabControl : TabControl
    {
        public ConferencesTabControl()
        {
            Visibility = System.Windows.Visibility.Hidden;
        }

        public static readonly DependencyProperty ConferencesSourceProperty =
            DependencyProperty.Register("ConferencesSource", typeof (ObservableCollection<ConferenceViewModel>),
                                        typeof (ConferencesTabControl),
                                        new PropertyMetadata(ConferencesSourceCollectionChangedStatic));

        public static readonly DependencyProperty PrivatesSourceProperty =
            DependencyProperty.Register("PrivatesSource", typeof (ObservableCollection<PrivateViewModel>),
                                        typeof (ConferencesTabControl),
                                        new PropertyMetadata(PrivatesSourceCollectionChangedStatic));

        public static readonly DependencyProperty SelectedConferenceViewModelProperty =
            DependencyProperty.Register("SelectedConferenceViewModel", typeof (ConferenceViewModel),
                                        typeof (ConferencesTabControl), new UIPropertyMetadata(null));

        public static readonly DependencyProperty SelectedPrivateViewModelProperty =
            DependencyProperty.Register("SelectedPrivateViewModel", typeof (PrivateViewModel),
                                        typeof (ConferencesTabControl), new UIPropertyMetadata(null));

        public ObservableCollection<ConferenceViewModel> ConferencesSource
        {
            get { return (ObservableCollection<ConferenceViewModel>) GetValue(ConferencesSourceProperty); }
            set { SetValue(ConferencesSourceProperty, value); }
        }

        public ObservableCollection<PrivateViewModel> PrivatesSource
        {
            get { return (ObservableCollection<PrivateViewModel>) GetValue(PrivatesSourceProperty); }
            set { SetValue(PrivatesSourceProperty, value); }
        }

        public ConferenceViewModel SelectedConferenceViewModel
        {
            get { return (ConferenceViewModel) GetValue(SelectedConferenceViewModelProperty); }
            set { SetValue(SelectedConferenceViewModelProperty, value); }
        }

        public PrivateViewModel SelectedPrivateViewModel
        {
            get { return (PrivateViewModel) GetValue(SelectedPrivateViewModelProperty); }
            set { SetValue(SelectedPrivateViewModelProperty, value); }
        }

        protected override void OnSelectionChanged(SelectionChangedEventArgs e)
        {
            base.OnSelectionChanged(e);


            SelectedPrivateViewModel = null;
            SelectedConferenceViewModel = null;

            if (SelectedItem is not TabItem item)
                return;


            if (item.Content is ConferenceView conferenceView)
            {
                SelectedConferenceViewModel = conferenceView.ConferenceViewModel;
                return;
            }

            if (item.Content is PrivateView privateView)
            {
                SelectedPrivateViewModel = privateView.ConferenceViewModel;
                return;
            }
        }

        private void SubscribeToConferenceSourceChanges()
        {
            if (ConferencesSource != null)
            {
                ConferencesSource.CollectionChanged += ConferencesSourceCollectionChanged;
                ConferencesSource.ForEach(AddConference);
            }
        }

        private void SubscribeToPrivateSourceChanges()
        {
            if (PrivatesSource != null)
            {
                PrivatesSource.CollectionChanged += PrivatesSourceCollectionChanged;
                PrivatesSource.ForEach(AddPrivate);
            }
        }

        private void ConferencesSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                AddConference(e.NewItems[0] as ConferenceViewModel);
            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (ConferenceViewModel item in e.OldItems)
                {
                    TabItem oldItem =
                        Items.OfType<TabItem>().FirstOrDefault(
                            i => i.Content is ConferenceView && ((ConferenceView) i.Content).ConferenceViewModel == item);
                    Items.Remove(oldItem);
                    Cleanup();
                }
            ControlVisibility();
        }

        private void ControlVisibility()
        {
            if (!Items.IsNullOrEmpty() && Visibility == Visibility.Hidden)
                Visibility = Visibility.Visible;
            else if (Items.IsNullOrEmpty())
                Visibility = Visibility.Hidden;
        }

        private void PrivatesSourceCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
                AddPrivate(e.NewItems[0] as PrivateViewModel);
            if (e.Action == NotifyCollectionChangedAction.Remove)
                foreach (PrivateViewModel item in e.OldItems)
                {
                    TabItem oldItem =
                        Items.OfType<TabItem>().FirstOrDefault(
                            i => i.Content is PrivateView && ((PrivateView)i.Content).ConferenceViewModel == item);
                    Items.Remove(oldItem);
                    Cleanup();
                }
            ControlVisibility();
        }

        private void Cleanup()
        {
            GC.Collect();
            GC.SuppressFinalize(this);
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        private ContextMenu GetContextMenu(string name)
        {
            return FindResource(name) as ContextMenu;
        }

        private void AddConference(ConferenceViewModel conference)
        {
            var view = new ConferenceView {ConferenceViewModel = conference};
            conference.View = view;
            var tab = new TabItem
                          {
                              Content = view,
                              DataContext = conference,
                          };

            Binding binding = new Binding("IsActive");
            binding.Mode = BindingMode.OneWayToSource;
            binding.Source = conference;
            tab.SetBinding(TabItem.IsSelectedProperty, binding);

            tab.SetResourceReference(HeaderedContentControl.HeaderTemplateProperty, "chatTabTemplate");
            //tab.ContextMenu = GetContextMenu("conferenceMenu");
            Items.Add(tab);
            SelectedItem = tab;
            ControlVisibility();
        }

        private void AddPrivate(PrivateViewModel privateModel)
        {
            var view = new PrivateView {ConferenceViewModel = privateModel};
            privateModel.View = view;
            var tab = new TabItem
                          {
                              Content = view,
                              DataContext = privateModel,
                          };

            Binding binding = new Binding("IsActive");
            binding.Mode = BindingMode.OneWayToSource;
            binding.Source = privateModel;
            tab.SetBinding(TabItem.IsSelectedProperty, binding);

            tab.SetResourceReference(HeaderedContentControl.HeaderTemplateProperty, "chatTabTemplate");
            //tab.ContextMenu = GetContextMenu("privateMenu");
            Items.Add(tab);
            ContextMenuServiceExtensions.SetDataContext(this, privateModel);
            SelectedItem = tab;
            ControlVisibility();
        }

        // Using a DependencyProperty as the backing store for ConferencesSource.  This enables animation, styling, binding, etc...

        private static void ConferencesSourceCollectionChangedStatic(DependencyObject d,
                                                                     DependencyPropertyChangedEventArgs e)
        {
            var tabcontrol = d as ConferencesTabControl;
            tabcontrol.SubscribeToConferenceSourceChanges();
        }

        private static void PrivatesSourceCollectionChangedStatic(DependencyObject d,
                                                                  DependencyPropertyChangedEventArgs e)
        {
            var tabcontrol = d as ConferencesTabControl;
            tabcontrol.SubscribeToPrivateSourceChanges();
        }
    }
}