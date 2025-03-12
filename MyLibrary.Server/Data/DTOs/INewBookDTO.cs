namespace MyLibrary.Server.Data.DTOs
{
    public interface INewBookDTO
    {
        IBookDTO Book { get; }
        int Quantity { get; }
    }
    public interface INewBookDTO<TId> where TId : IEquatable<TId>
    {
        IBookDTO<TId> Book { get; }
        int Quantity { get; }
    }
}
