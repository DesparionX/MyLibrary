namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface IBorrow : IEntity<int>
    {
        public string BookId { get; set; }
        public string UserId { get; set; }
        public DateTime DateBorrowed { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}
