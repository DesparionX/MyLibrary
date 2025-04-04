using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public interface IBookHandler<IBook>
    {
        public Task<ITaskResult> GetAllBooks();
        public Task<ITaskResult> FindBookByISBN(string ISBN);
        public Task<ITaskResult> FindBookById<TId>(TId id);
        public Task<ITaskResult> AddBookAsync(INewBook<BookDTO> newBookDto);
        public Task<ITaskResult> UpdateBookAsync(IBookDTO bookDto);
        public Task<ITaskResult> DeleteBookAsync(Guid id);
    }
}
