using System;
using System.Drawing;
using System.IO;
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
        public byte[]? Bitmap { get; set; }

        [XmlIgnore]
        public MemoryStream Stream { get; set; }
    }
}
