namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface ISubscriptionTier<TId> : IEntity<TId> where TId : IEquatable<TId>
    {
        public string? Tier { get; set; }
        public int? Months { get; set; }
        public decimal? Cost { get; set; }
        public bool? CanBorrow { get; set; }
        public int? BorrowLimit { get; set; }
        public float? Discount { get; set; }
    }
}
