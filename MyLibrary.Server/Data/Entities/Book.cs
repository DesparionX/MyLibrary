using MyLibrary.Server.Data.Entities.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace MyLibrary.Server.Data.Entities
{
    public class Book : IBook<Guid>
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ISBN { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string? Picture { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Publisher { get; set; } = string.Empty;
        public decimal BasePrice { get; set; } = 0;
        public float Discount { get; set; } = 0;
        public int Pages { get; set; } = 0;
        public bool IsAvailable { get; set; } = true;
    }
}
