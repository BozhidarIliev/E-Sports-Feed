using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsFeed.Data.Models
{
    public class Market : IActiveEntity
    {

        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public string ID { get; set; }
        public bool IsActive { get; set; } = true;
        public string Name { get; set; }

        public bool IsLive { get; set; }

        public List<Odd> Odds { get; set; }
    }
}
