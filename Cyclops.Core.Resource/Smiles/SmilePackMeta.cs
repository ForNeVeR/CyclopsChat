using System.Xml.Serialization;
using Cyclops.Core.Smiles;

namespace Cyclops.Core.Resource.Smiles
{
    public class SmilePackMeta : ISmilePackMeta
    {
        [XmlElement("name")]
        public string Name { get; set; }

        [XmlElement("version")]
        public string Version { get; set; }

        [XmlElement("creation")]
        public string Creation { get; set; }

        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("home")]
        public string Home { get; set; }

        [XmlElement("author")]
        public string Author { get; set; }
    }
}