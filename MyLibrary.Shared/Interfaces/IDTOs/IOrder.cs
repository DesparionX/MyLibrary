namespace MyLibrary.Shared.Interfaces.IDTOs
{
    public interface IOrder
    {
        public ICollection<string>? ItemsId { get; set; }
        public string? ItemISBN { get; set; }
        public string? ItemName { get; set; }
        public int Quantity { get; set; }
    }
}
