using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public interface IBookDTO<TId> : IDTO<TId, IBook<TId>>, IBook<TId> where TId : IEquatable<TId>
    {

    }
    public interface IBookDTO : IDTO<Guid, IBook<Guid>>, IBook<Guid>
    {

    }
    
}
