using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public interface INewBook<TBookDTO> where TBookDTO : IBookDTO
    {
        TBookDTO Book { get; }
        int Quantity { get; }
    }
}
