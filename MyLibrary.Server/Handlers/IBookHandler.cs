using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public interface IBookHandler<IBook>
    {
        public Task<ITaskResponse> GetAllBooks();
        public Task<ITaskResponse> FindBookByISBN(string ISBN);
        public Task<ITaskResponse> FindBookById<TId>(TId id);
        public Task<ITaskResponse> AddBookAsync(INewBookDTO newBookDto);
        public Task<ITaskResponse> AddBookAsync<TId>(INewBookDTO<TId> newBookDto) where TId : IEquatable<TId>;
        public Task<ITaskResponse> UpdateBookAsync(IBookDTO bookDto);
        public Task<ITaskResponse> UpdateBookAsync<TId>(IBookDTO<TId> bookDto) where TId : IEquatable<TId>;
        public Task<ITaskResponse> DeleteBookAsync(Guid id);
        public Task<ITaskResponse> DeleteBookAsync<TId>(TId id);
    }
}
