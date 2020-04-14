using System.Collections.Generic;
using System.Xml.Serialization;

namespace TGDBHashTool.Models.SimpleHashes
{
    [XmlRoot(ElementName = "info")]
    public class SimpleHashes
    {
        [XmlElement(ElementName = "hash")]
        public List<SimpleHash> Hashes = new List<SimpleHash>();
    }
}
