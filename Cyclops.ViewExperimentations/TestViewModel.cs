using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
                    AvatarUrl = @"C:\Avatars\" + random.Next(1, 8) + ".jpg"
                });
            }
        }
        
        public ObservableCollection<Member> Members { get; set; }
    }
}
