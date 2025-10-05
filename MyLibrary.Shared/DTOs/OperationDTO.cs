using MyLibrary.Server.Data.Entities;
using MyLibrary.Shared.Interfaces.IDTOs;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyLibrary.Server.Data.DTOs
{
    public class OperationDTO : IOperationDTO
    {
        public int Id { get; set; }

        public string? OperationName { get; set; }

        [NotMapped, JsonIgnore]
        public ICollection<IOrder>? OrderList
        {
            get => OrderListInternal?.Cast<IOrder>().ToList();
            set => OrderListInternal = value?.Cast<Order>().ToList();
        }

        [JsonProperty("orderList")]
        public List<Order>? OrderListInternal { get; set; }

        public string? UserId { get; set; }

        public string? UserName { get; set; }

        public string? UserRole { get; set; }
        public string? BorrowerId { get; set; }

        public decimal? TotalPrice { get; set; }

        public DateTime? OperationDate { get; set; }
        
    }
}
