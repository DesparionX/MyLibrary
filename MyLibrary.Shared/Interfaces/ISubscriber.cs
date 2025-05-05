namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface ISubscriber
    {
        public int BorrowLimit { get; set; }
        public bool CanBorrow { get; set; }
        public float Discount { get; set; }
    }
}
