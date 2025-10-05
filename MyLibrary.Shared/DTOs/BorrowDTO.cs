using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Shared.DTOs
{
    public class BorrowDTO : IBorrowDTO
    {
        public int Id { get; set; }
        public required string BookId { get; set; } = string.Empty;
        public required string UserId { get; set; } = string.Empty;
        public DateTime BorrowedAt { get; set; } = DateTime.UtcNow;
        public DateTime? ReturnedAt { get; set; } = null;
    }
}
