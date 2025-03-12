
namespace MyLibrary.Server.Data.Entities
{
    public class Operation : IOperation<int>
    {
        public int Id { get; }
        public string? OperationName { get; }
        public string? ArticleId { get; }

        public string? ArticleISBN { get; }

        public string? ArticleName { get; }

        public string? StaffId { get; }

        public string? StaffName { get; }

        public string? ClientId { get; }

        public string? ClientName { get; }

        public int? Quantity { get; }

        public decimal? TotalPrice { get; }

        public DateTime? OperationDate { get; }
    }
}
