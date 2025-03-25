using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public interface IBookHandler<IBook>
    {
        public Task<ITaskResult> GetAllBooks();
        public Task<ITaskResult> FindBookByISBN(string ISBN);
        public Task<ITaskResult> FindBookById<TId>(TId id);
        public Task<ITaskResult> AddBookAsync(INewBookDTO newBookDto);
        public Task<ITaskResult> AddBookAsync<TId>(INewBookDTO<TId> newBookDto) where TId : IEquatable<TId>;
        public Task<ITaskResult> UpdateBookAsync(IBookDTO bookDto);
        public Task<ITaskResult> UpdateBookAsync<TId>(IBookDTO<TId> bookDto) where TId : IEquatable<TId>;
        public Task<ITaskResult> DeleteBookAsync(Guid id);
        public Task<ITaskResult> DeleteBookAsync<TId>(TId id);
    }
}
