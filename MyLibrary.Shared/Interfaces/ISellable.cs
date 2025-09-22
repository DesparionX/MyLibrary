namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface ISellable
    {
        public string? Picture { get; set; }
        public decimal BasePrice { get; set; }
        public float Discount { get; set; }
    }
}
