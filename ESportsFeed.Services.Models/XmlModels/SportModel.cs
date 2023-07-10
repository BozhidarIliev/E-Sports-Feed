using System.Xml.Serialization;

namespace ESportsFeed.Services.Models
{
    [XmlRoot("Sport", Namespace = "")]
    public class SportModel
    {
        [XmlAttribute("ID")]
        public int ID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlElement("Event")]
        public List<EventModel> Events { get; set; }
    }
}
