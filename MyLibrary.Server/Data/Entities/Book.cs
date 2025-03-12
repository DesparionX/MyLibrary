namespace MyLibrary.Server.Data.Entities
{
    public class Book : IBook<Guid>
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string ISBN { get; set; } = "";
        public string Genre { get; set; } = "";
        public string Title { get; set; } = "";
        public string Description { get; set; } = "";
        public string Author { get; set; } = "";
        public string Publisher { get; set; } = "";
        public decimal BasePrice { get; set; } = 0;
        public float Discount { get; set; } = 0;
        public int Pages { get; set; } = 0;
    }
}
