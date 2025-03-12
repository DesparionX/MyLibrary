using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public interface IOperationDTO<TId> : IDTO<TId, IOperation<TId>>, IOperation<TId> where TId : IEquatable<TId>
    {

    }
    public interface IOperationDTO : IDTO<int, IOperation<int>>, IOperation<int>
    {

    }
}
