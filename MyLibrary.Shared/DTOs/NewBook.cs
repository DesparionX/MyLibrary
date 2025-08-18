using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.Server.Data.DTOs
{
    public class NewBook : INewBook<BookDTO>
    {
        public required BookDTO Book {get; set; }

        public int Quantity { get; set; }
    }
}
