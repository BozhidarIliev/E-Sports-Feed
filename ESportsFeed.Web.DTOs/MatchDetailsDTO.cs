using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsFeed.Web.DTOs
{
    public class MatchDetailsDTO
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public List<MarketDTO> ActiveMarkets { get; set; }
        public List<MarketDTO> InactiveMarkets { get; set; }
    }
}
