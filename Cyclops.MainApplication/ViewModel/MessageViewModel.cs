using System.Windows.Documents;
using Cyclops.Core;
using Cyclops.MainApplication.MessageDecoration;

namespace Cyclops.MainApplication.ViewModel
{
    /// <summary>
    /// View model for message
    /// </summary>
    public class MessageViewModel : ViewModelBaseEx
    {
        public MessageViewModel(IConferenceMessage msg)
        {
            RawMessage = msg;
            Paragraph = MessagePresenter.Present(msg);
        }

        public IConferenceMessage RawMessage { get; private set; }
        public Paragraph Paragraph { get; private set; }

        public override string ToString()
        {
            return RawMessage.ToString();
        }
    }
}
