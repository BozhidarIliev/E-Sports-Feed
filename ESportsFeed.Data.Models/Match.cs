using System.ComponentModel.DataAnnotations.Schema;

namespace ESportsFeed.Data.Models
{
    public class Match : IActiveEntity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public string ID { get; set; }

        public bool IsActive { get; set; } = true;

        public string Name { get; set; }

        public Web.Common.MatchType MatchType { get; set; }

        public DateTime StartDate { get; set; }

        public List<Market> Markets { get; set; }
    }
}
