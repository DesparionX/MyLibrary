namespace MyLibrary.Server.Data.Entities
{
    public interface IUser<TId> : IEntity<TId> where TId : IEquatable<TId>
    {
        string? UserName { get; }
        string? FirstName { get; }
        string? LastName { get; }
        public DateTime? BirthDate { get; }
        string? Avatar {  get; }
        string? Email { get; }
        string? PhoneNumber { get; }
        string? Country { get; }
        string? City { get; }
        string? Address { get; }
        int BookLimit { get; }
        int Rating { get; }

    }
}
