using System.ComponentModel.DataAnnotations.Schema;

namespace ESportsFeed.Data.Models
{
    public interface IActiveEntity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        string ID { get; set; }
        bool IsActive { get; set; }
    }
}
