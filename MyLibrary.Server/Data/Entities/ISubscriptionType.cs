namespace MyLibrary.Server.Data.Entities
{
    public interface ISubscriptionType<T> : IEntity<T> where T : IEquatable<T>
    {
        public string Type { get; }
        public decimal MonthlyCost { get; }
    }
}
