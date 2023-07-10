using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsFeed.Web.DTOs
{
    public class OddDTO
    {
        public string Name { get; set; }
        public decimal Value { get; set; }
        public decimal? SpecialBetValue { get; set; }
    }
}
