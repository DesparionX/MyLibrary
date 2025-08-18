using MyLibrary.Server.Data.Entities;
using MyLibrary.Shared.Interfaces.IDTOs;
namespace MyLibrary.Server.Data.DTOs
{
    public class BookDTO : IBookDTO
    {
        public Guid Id { get; set; }

        public required string ISBN { get; set; }

        public required string Genre { get; set; }

        public  required string Title { get; set; }

        public required string Description { get; set; }

        public required string Author { get; set; }

        public required string Publisher { get; set; }

        public int Pages { get; set; }

        public decimal BasePrice { get; set; }

        public float Discount { get; set; }
        public bool IsAvailable { get; set; }
    }
}
