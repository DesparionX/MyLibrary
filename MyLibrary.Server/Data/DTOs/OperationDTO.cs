using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Data.DTOs
{
    public class OperationDTO : IOperationDTO
    {
        public int Id { get; set; }

        public string? OperationName { get; set; }

        public string? ArticleId { get; set; }

        public string? ArticleISBN { get; set; }

        public string? ArticleName { get; set; }

        public string? StaffId { get; set; }

        public string? StaffName { get; set; }

        public string? ClientId { get; set; }

        public string? ClientName { get; set; }

        public int? Quantity { get; set; }

        public decimal? TotalPrice { get; set; }

        public DateTime? OperationDate { get; set; }
    }
}
