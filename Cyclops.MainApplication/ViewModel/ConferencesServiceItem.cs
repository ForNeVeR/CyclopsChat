using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cyclops.MainApplication.ViewModel
{
    public class ConferencesServiceItem
    {
        [XmlAttribute]
        public string DisplayName { get; set; }
        [XmlAttribute]
        public string ConferenceService { get; set; }
    }
}
