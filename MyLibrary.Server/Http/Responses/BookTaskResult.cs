using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Http.Responses
{
    public class BookTaskResult : ITaskResult
    {
        public string? Message { get; }
        public bool Succeeded { get; }
        public int StatusCode { get; }
        public ICollection<BookDTO>? Books { get; }
        public BookDTO? Book { get; }

        public BookTaskResult(bool succeeded, int statusCode, string? message = "", ICollection<BookDTO>? books = null, BookDTO? book = null)
        {
            Succeeded = succeeded;
            Message = message;
            StatusCode = statusCode;
            Books = books;
            Book = book;
        }
    }
}
