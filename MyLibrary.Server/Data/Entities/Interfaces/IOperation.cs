using MyLibrary.Server.Data.DTOs.Interfaces;

namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface IOperation<TId> : IEntity<TId>
        where TId : IEquatable<TId>
    {
        string? OperationName { get; set; }
        ICollection<IOrder>? OrderList { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserRole { get; set; }
        decimal? TotalPrice { get; set; }
        DateTime? OperationDate { get; set; }
    }
}
