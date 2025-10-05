
using MyLibrary.Server.Data.DTOs;
using MyLibrary.Shared.Interfaces.IDTOs;
using MyLibrary.Server.Data.Entities.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibrary.Server.Data.Entities
{
    public class Operation : IOperation<int>
    {
        public int Id { get; set; }

        public string? OperationName { get; set; }

        [NotMapped]
        public ICollection<IOrder>? OrderList
        {
            get => OrderListInternal?.Cast<IOrder>().ToList();
            set => OrderListInternal = value?.Cast<Order>().ToList();
        }
        public List<Order>? OrderListInternal { get; set; }

        public string? UserId { get; set; }

        public string? UserName { get; set; }

        public string? UserRole { get; set; }
        public string? BorrowerId { get; set; }

        public decimal? TotalPrice { get; set; }

        public DateTime? OperationDate { get; set; }
        
    }
}
