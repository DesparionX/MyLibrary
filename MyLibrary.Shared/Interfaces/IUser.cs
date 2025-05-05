namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface IUser<TId> : ISubscriber, IEntity<TId>
        where TId : IEquatable<TId>
    {
        string? UserName { get; set; }
        string? FirstName { get; set; }
        string? LastName { get; set; }
        public DateTime? BirthDate { get; set; }
        string? Avatar {  get; set; }
        string? Email { get; set; }
        string? PhoneNumber { get; set; }
        string? Country { get; set; }
        string? City { get; set; }
        string? Address { get; set; }
        int Rating { get; set; }

    }
}
