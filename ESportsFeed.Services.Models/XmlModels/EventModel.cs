using System.Xml.Serialization;

namespace ESportsFeed.Services.Models
{
    [XmlRoot("Event", Namespace = "")]
    public class EventModel
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("IsLive")]
        public bool IsLive { get; set; }

        [XmlAttribute("CategoryID")]
        public string CategoryID { get; set; }

        [XmlElement("Match")]
        public List<MatchModel> Matches { get; set; }
    }
}
