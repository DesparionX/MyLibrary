namespace MyLibrary.Server.Data.Entities
{
    public interface ISubscription<T, TUserId> : IEntity<T>
        where T : IEquatable<T> 
        where TUserId : IEquatable<TUserId>
    {
        public ISubscriptionType<T> Type { get; }
        public TUserId UserId { get; }
        public int Months { get; }
        public DateTime ExpireDate { get; }

    }
}
