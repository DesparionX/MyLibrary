using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Shared.Interfaces.IDTOs
{
    public interface INewBook<TBookDTO> where TBookDTO : IBookDTO
    {
        TBookDTO Book { get; }
        int Quantity { get; }
    }
}
