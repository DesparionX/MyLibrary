namespace MyLibrary.Shared.Interfaces.IDTOs
{
    public interface IOrder
    {
        public string? ItemId { get; set; }
        public string? ItemISBN { get; set; }
        public string? ItemName { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }
    }
}
