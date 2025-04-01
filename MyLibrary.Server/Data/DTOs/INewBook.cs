using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public interface INewBook
    {
        IBookDTO Book { get; }
        int Quantity { get; }
    }
}
