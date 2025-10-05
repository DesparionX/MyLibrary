using MyLibrary.Server.Data.DTOs;
using MyLibrary.Shared.Interfaces.IDTOs;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers.Interfaces
{
    public interface IBookHandler<IBook>
    {
        public Task<ITaskResult> GetAllBooks();
        public Task<ITaskResult> FindBookByISBN(string ISBN, bool? isAvailable = default);
        public Task<ITaskResult> FindBookById(string id);
        public Task<ITaskResult> AddBookAsync(INewBook<BookDTO> newBookDto);
        public Task<ITaskResult> UpdateBookAsync(IBookDTO bookDto);
        public Task<ITaskResult> UpdateBookAvailabilityAsync(ICollection<string> ids, bool isAvailable);
        public Task<ITaskResult> DeleteBookAsync(Guid id);
    }
}
