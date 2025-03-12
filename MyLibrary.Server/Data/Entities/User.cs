using Microsoft.AspNetCore.Identity;

namespace MyLibrary.Server.Data.Entities
{
    public class User : IdentityUser, IUser<string>
    {
        public override string Id { get; set; } = Guid.NewGuid().ToString();
        public override string? UserName { get; set; }
        public string? FirstName { get; }

        public string? LastName { get; }

        public DateTime? BirthDate { get; }

        public string? Avatar { get; }
        public override string? Email { get; set; }
        public override string? PhoneNumber { get; set; }

        public string? Country { get; }

        public string? City { get; }

        public string? Address { get; }

        public int BookLimit { get; }

        public int Rating { get; }
    }
}
