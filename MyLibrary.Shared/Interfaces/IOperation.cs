using MyLibrary.Shared.Interfaces.IDTOs;

namespace MyLibrary.Server.Data.Entities.Interfaces
{
    public interface IOperation<TId> : IEntity<TId>
        where TId : IEquatable<TId>
    {
        public string? OperationName { get; set; }
        public ICollection<IOrder>? OrderList { get; set; }
        public string? UserId { get; set; }
        public string? UserName { get; set; }
        public string? UserRole { get; set; }
        public string? BorrowerId { get; set; }
        public decimal? TotalPrice { get; set; }
        public DateTime? OperationDate { get; set; }
    }
}
