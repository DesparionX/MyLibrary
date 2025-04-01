namespace MyLibrary.Server.Data.DTOs
{
    public class NewBook : INewBook
    {
        public IBookDTO Book {get; set; }

        public int Quantity { get; set; }
    }
}
