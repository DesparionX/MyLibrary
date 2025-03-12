using MyLibrary.Server.Data.Entities;
namespace MyLibrary.Server.Data.DTOs
{
    public class BookDTO : IBookDTO<Guid>
    {
        public Guid Id { get; set; }

        public IBook<Guid> Entity { get; set; }

        public string ISBN { get; set; }

        public string Genre { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string Author { get; set; }

        public string Publisher { get; set; }

        public int Pages { get; set; }

        public decimal BasePrice { get; set; }

        public float Discount { get; set; }
    }
}
