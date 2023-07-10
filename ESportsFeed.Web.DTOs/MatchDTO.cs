namespace ESportsFeed.Web.DTOs
{
    public class MatchDTO
    {
        public string Name { get; set; }
        public DateTime StartDate { get; set; }
        public List<PreviewMarketDTO> ActivePreviewMarkets { get; set; }
    }
}