using MyLibrary.Server.Data.DTOs.Interfaces;

namespace MyLibrary.Server.Data.DTOs
{
    public class NewBook : INewBook<BookDTO>
    {
        public BookDTO Book {get; set; }

        public int Quantity { get; set; }
    }
}
