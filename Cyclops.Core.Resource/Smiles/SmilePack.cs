using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using Cyclops.Core.Smiles;

namespace Cyclops.Core.Resource.Smiles
{
    [XmlRoot(ElementName="icondef")]
    public class SmilePack : ISmilePack
    {
        [XmlIgnore]
        public ISmilePackMeta Meta => MetaForDeserialization;

        [XmlElement("meta", typeof(SmilePackMeta))]
        public SmilePackMeta MetaForDeserialization { get; set; }

        [XmlIgnore]
        public ISmile[] Smiles => SmilesForDeserialization;

        [XmlElement("icon", typeof(Smile))]
        public Smile?[] SmilesForDeserialization { get; set; }

        //NOTE: XmlSerializer can't deserialize Interface properties :( (tried to use XmlInclude and etc)
    }
}
