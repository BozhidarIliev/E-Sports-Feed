using System.Xml.Serialization;

namespace ESportsFeed.Services.Models
{
    [XmlRoot("XmlSports", Namespace = "")]

    public class XmlSportsWrapper
    {
        [XmlElement("Sport")]
        public List<SportModel> Sport { get; set; }
    }
}
