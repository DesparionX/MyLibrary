namespace MyLibrary.Server.Data.Entities
{
    public interface IOperation<TId> : IEntity<TId> where TId : IEquatable<TId>
    {
        string? OperationName { get; }
        string? ArticleId { get; }
        string? ArticleISBN { get; }
        string? ArticleName { get; }
        string? StaffId { get; }
        string? StaffName {  get; }
        string? ClientId { get; }
        string? ClientName { get; }
        int? Quantity { get; }
        decimal? TotalPrice { get; }
        DateTime? OperationDate { get; }
    }
}
