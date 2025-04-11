namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface ISubscriptionTier : IEntity<int>
    {
        public string? Tier { get; set; }
        public decimal? MonthlyCost { get; set; }
    }
}
