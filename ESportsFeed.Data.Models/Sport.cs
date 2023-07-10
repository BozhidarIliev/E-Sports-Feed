using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ESportsFeed.Data.Models
{
    public class Sport : IActiveEntity
    {
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public string ID { get; set; }

        public bool IsActive { get; set; } = true;

        public string Name { get; set; }

        public List<Event> Events { get; set; }
    }
}
