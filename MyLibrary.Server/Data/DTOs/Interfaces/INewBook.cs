using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs.Interfaces
{
    public interface INewBook<TBookDTO> where TBookDTO : IBookDTO
    {
        TBookDTO Book { get; }
        int Quantity { get; }
    }
}
