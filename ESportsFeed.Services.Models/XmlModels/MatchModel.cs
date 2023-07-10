using System.Xml.Serialization;

namespace ESportsFeed.Services.Models
{
    [XmlRoot("Match", Namespace = "")]
    public class MatchModel
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("MatchType")]
        public MatchTypeModel MatchType { get; set; }

        [XmlAttribute("StartDate")]
        public DateTime StartDate { get; set; }

        [XmlElement("Bet")]
        public List<MarketModel> Markets { get; set; }
    }
}
