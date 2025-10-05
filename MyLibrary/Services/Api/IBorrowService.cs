using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Services.Api
{
    public interface IBorrowService
    {
        Task<ITaskResult> GetBorrowHistoryAsync();
        Task<ITaskResult> GetUserBorrowsAsync(string userId);
        Task<ITaskResult> GetBooksBorrowsAsync(string bookId, bool? isReturned = default);
        Task<ITaskResult> BorrowBookAsync(string bookId, string userId);
        Task<ITaskResult> ReturnBookAsync(string bookId, string userId);
    }
}
