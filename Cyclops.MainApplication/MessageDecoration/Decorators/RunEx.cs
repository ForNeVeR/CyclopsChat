using System.Windows.Documents;

namespace Cyclops.MainApplication.MessageDecoration.Decorators
{
    public class RunEx : Run
    {
        public RunEx(string text, MessagePartType type) : base(text)
        {
            MessagePartType = type;
        }

        public MessagePartType MessagePartType { get; set; }
    }
}