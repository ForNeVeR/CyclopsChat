using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cyclops.MainApplication.ViewModel
{
    /// <summary>
    /// For design time. 
    /// Makes ConferenceView more blendable!
    /// </summary>
    public class ConferenceViewModelDesignTime
    {
        public ConferenceViewModelDesignTime()
        {
            var members = new List<Member>();
            for (int i = 0; i < 15; i++)
            {
                members.Add(new Member
                                {
                                    AvatarUrl = @"..\Resources\testavatar.png",
                                    IsModer = false,
                                    Nick = "Username" + i,
                                    StatusText = "Some status text some status text"
                                });
            }

            ConferenceViewModel = new ConferenceViewModelStub
                                      {
                                          Conference = new ConferenceStub
                                                           {
                                                               Members = members
                                                           }
                                      };
        }

        public ConferenceViewModelStub ConferenceViewModel { get; set; }

        public class ConferenceViewModelStub
        {
            public ConferenceStub Conference { get; set; }
        }
        
        public class ConferenceStub
        {
            public IEnumerable<Member> Members { get; set; }
        }

        public class Member
        {
            public string AvatarUrl { get; set; }
            public string Nick { get; set; }
            public string StatusText { get; set; }
            public bool IsModer { get; set; }
        }
    }
}
