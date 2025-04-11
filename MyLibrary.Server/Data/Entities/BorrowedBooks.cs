
using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.Entities
{
    public class BorrowedBooks : IBorrowedBooks
    {
        public int Id { get; set; }
        public string BookId { get; set; }
        public string UserId { get; set; }
        public DateTime DateBorrowed { get; set; }
    }
}
