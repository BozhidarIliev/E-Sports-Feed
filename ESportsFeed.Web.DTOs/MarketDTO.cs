using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsFeed.Web.DTOs
{
    public class MarketDTO
    {
        public string MarketName { get; set; }
        public List<OddDTO> ActiveOdds { get; set; }
        public List<OddDTO> InactiveOdds { get; set; }
    }
}
