using MyLibrary.Server.Data.DTOs.Interfaces;

namespace MyLibrary.Server.Data.DTOs
{
    public class Order : IOrder
    {
        public ICollection<string>? ItemsId { get; set; }
        public string? ItemISBN { get; set; }
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
    }
}
