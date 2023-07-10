namespace ESportsFeed.Services.Messaging
{
    public class HidingMessage
    {
        public string EntityId { get; set; }

        public string EntityType { get; set; }

        public DateTime DateTime{ get; set; }
    }

    public class MatchChangesMessage
    {
        public string MatchId { get; set; }
        public string MatchType { get; set; }
        public DateTime StartDate { get; set; }
    }

    public class OddChangesMessage
    {
        public string OddId { get; set; }
        public decimal Value { get; set; }
    }
}