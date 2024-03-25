using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Cyclops.MainApplication.ViewModel;

namespace Cyclops.MainApplication.Controls
{
    public class ChatFlowDocument : FlowDocument
    {
        public ChatFlowDocument()
        {
            FocusManager.SetIsFocusScope(this, true);
            Loaded += (s, e) => ScrollToBottom();
        }
        
        // Using a DependencyProperty as the backing store for Messages.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty MessagesProperty =
            DependencyProperty.Register("Messages", typeof (ObservableCollection<MessageViewModel>),
                                        typeof (ChatFlowDocument), new UIPropertyMetadata(OnInitializeMessagesStatic));
        
        public ObservableCollection<MessageViewModel> Messages
        {
            get { return (ObservableCollection<MessageViewModel>) GetValue(MessagesProperty); }
            set { SetValue(MessagesProperty, value); }
        }

        private static void OnInitializeMessagesStatic(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is ChatFlowDocument)
                ((ChatFlowDocument) d).OnInitializeMessages();
        }
        
        private void OnInitializeMessages()
        {
            Messages.ForEach(msg => Blocks.Add(msg.Paragraph));
            Messages.CollectionChanged += (s, e) =>
                                              {
                                                  if (e.Action == NotifyCollectionChangedAction.Add)
                                                      e.NewItems.OfType<MessageViewModel>().ForEach(
                                                          i => Blocks.Add(i.Paragraph));
                                                  ScrollToBottom();
                                              };
            ScrollToBottom();
        }
        
        private void ScrollToBottom()
        {
            if (Parent is ChatFlowDocumentScrollViewer)
            {
                ScrollViewer scrollViewer = ((ChatFlowDocumentScrollViewer) Parent).ScrollViewer;
                if (scrollViewer != null)
                    scrollViewer.ScrollToBottom();
            }
        }

    }
}