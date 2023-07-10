using System.Xml.Serialization;

namespace ESportsFeed.Services.Models
{
    [XmlRoot("Market", Namespace = "")]
    public class MarketModel
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("IsLive")]
        public bool IsLive { get; set; }

        [XmlElement("Odd")]
        public List<OddModel> Odds { get; set; }
    }
}
