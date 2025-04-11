using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MyLibrary.Server.Data.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Server.Data.Entities
{
    public class User : IdentityUser, IUser<string>
    {
        [Key]
        public override string Id { get; set; } = Guid.NewGuid().ToString();

        public override string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Avatar { get; set; }

        public override string? Email { get; set; }

        public override string? PhoneNumber { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }

        public int Rating { get; set; }

        public bool CanBorrow { get; set; }

        public int BorrowLimit { get; set; }
        
        public float Discount { get; set; }
    }
}
