using System.Collections.Generic;
using System.Xml.Serialization;

namespace TGDBHashTool.Models.SimpleHashes
{
    [XmlRoot(ElementName = "hash")]
    public class SimpleHash
    {
        [XmlAttribute(AttributeName = "crc32")]
        public string Crc32;

        [XmlIgnore]
        public List<int> TgdbId = new List<int>();

        [XmlAttribute(AttributeName = "tgdb")]
        public string TgdbIdCsv
        {
            get => string.Join(",", TgdbId.ToArray());
            set
            {
                TgdbId.Clear();
                foreach (var id in value.Split(','))
                {
                    int parsedId;

                    if (int.TryParse(id.Trim(), out parsedId))
                    {
                        TgdbId.Add(parsedId);
                    }
                }
            }
        }
    }
}
