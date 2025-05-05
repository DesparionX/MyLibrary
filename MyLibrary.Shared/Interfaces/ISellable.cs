namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface ISellable
    {
        public decimal BasePrice { get; set; }
        public float Discount { get; set; }
    }
}
