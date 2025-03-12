namespace MyLibrary.Server.Data.Entities
{
    public interface ISellable
    {
        public decimal BasePrice { get; }
        public float Discount { get; }
    }
}
