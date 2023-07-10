using System.Xml.Serialization;

namespace ESportsFeed.Services.Models
{
    [XmlRoot("Odd", Namespace = "")]
    public class OddModel
    {
        [XmlAttribute("ID")]
        public string ID { get; set; }

        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("Value")]
        public decimal Value { get; set; }

        [XmlAttribute("SpecialBetValue")]
        public double SpecialBetValue { get; set; }
    }
}
