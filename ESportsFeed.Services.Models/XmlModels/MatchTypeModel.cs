using System.Xml.Serialization;

namespace ESportsFeed.Services.Models
{
    public enum MatchTypeModel
    {
        [XmlEnum("PreMatch")]
        PreMatch,
        [XmlEnum("Live")]
        Live,
        [XmlEnum("OutRight")]
        OutRight
    }
}