using MyLibrary.Server.Data.DTOs.Interfaces;
using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public class UserDTO : IUserDTO
    {
        public string? Id { get; set; }

        public string? UserName { get; set; }

        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public DateTime? BirthDate { get; set; }

        public string? Avatar { get; set; }

        public string? Email { get; set; }

        public string? PhoneNumber { get; set; }

        public string? Country { get; set; }

        public string? City { get; set; }

        public string? Address { get; set; }

        public int BorrowLimit { get; set; }

        public bool CanBorrow { get; set; }

        public float Discount { get; set; }

        public int Rating { get; set; } = 0;
        
    }
}
