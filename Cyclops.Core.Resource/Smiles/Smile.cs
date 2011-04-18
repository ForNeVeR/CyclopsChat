using System.Drawing;
using System.Xml.Serialization;
using Cyclops.Core.Smiles;

namespace Cyclops.Core.Resource.Smiles
{
    public class Smile : ISmile
    {
        [XmlElement("text")]
        public string[] Masks { get; set; }

        [XmlElement("object")]
        public string File { get; set; }

        [XmlIgnore]
        public Bitmap Bitmap { get; set; }
    }
}
