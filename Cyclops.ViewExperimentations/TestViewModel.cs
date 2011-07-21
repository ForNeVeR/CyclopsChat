using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Cyclops.Core;
using GalaSoft.MvvmLight;

namespace Cyclops.ViewExperimentations
{
    public class TestViewModel : ViewModelBase
    {
        public TestViewModel()
        {
            var random = new Random(Environment.TickCount);
            Members = new ObservableCollection<Member>();
            for (int i = 0; i < 10; i++)
            {
                Members.Add(new Member
                {
                    Nick = "Some_nick " + i,
                    StatusText = "Some stupid meanless status text",
                });
            }

            Messages = new ObservableCollection<IConferenceMessage>();
            Messages.Add(new SystemConferenceMessage {Body = "Test"});
        }
        
        public ObservableCollection<Member> Members { get; set; }

        public ObservableCollection<IConferenceMessage> Messages { get; set; }
    }
}
