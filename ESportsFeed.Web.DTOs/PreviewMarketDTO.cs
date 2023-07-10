using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ESportsFeed.Web.DTOs
{
    public class PreviewMarketDTO
    {
        public string Name { get; set; }
        public List<OddDTO> ActiveOdds { get; set; }
    }
}
