namespace BountySystem.API.Entities
{
    public class Bounty
    {
        public int IssuerId { get; set; }
        public int TargetId { get; set; }

        public int Reward { get; set; }
        public string Reason { get; set; }
    }
}
