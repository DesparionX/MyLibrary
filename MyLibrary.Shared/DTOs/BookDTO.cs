using MyLibrary.Server.Data.Entities;
using MyLibrary.Shared.Interfaces.IDTOs;
namespace MyLibrary.Server.Data.DTOs
{
    public class BookDTO : IBookDTO
    {
        public Guid Id { get; set; }

        public string ISBN { get; set; }

        public string Genre { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public int Pages { get; set; }

        public decimal BasePrice { get; set; }

        public float Discount { get; set; }
        public bool IsAvailable { get; set; }
    }
}
