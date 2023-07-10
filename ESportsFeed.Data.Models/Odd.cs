using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsFeed.Data.Models
{
    public class Odd : IActiveEntity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public string ID { get; set; }

        public bool IsActive { get; set; } = true;
        public string Name { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Value { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal SpecialBetValue { get; set; }
    }
}
