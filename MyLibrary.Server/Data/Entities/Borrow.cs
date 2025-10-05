
using MyLibrary.Server.Data.Entities.Interfaces;

namespace MyLibrary.Server.Data.Entities
{
    public class Borrow : IBorrow
    {
        public int Id { get; set; }
        public required string BookId { get; set; }
        public required string UserId { get; set; }
        public DateTime DateBorrowed { get; set; }
        public DateTime? DateReturned { get; set; }
    }
}
