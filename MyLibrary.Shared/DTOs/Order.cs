using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.Server.Data.DTOs
{
    public class Order : IOrder
    {
        public string? ItemId { get; set; }
        public string? ItemISBN { get; set; }
        public string? ItemName { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
    }
}
