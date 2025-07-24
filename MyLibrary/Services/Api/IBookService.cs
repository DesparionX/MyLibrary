using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.Api
{
    public interface IBookService
    {
        public Task<ITaskResult> GetAllBooksAsync();
        public Task<ITaskResult> FindBookByIdAsync<T>(T bookId) where T : IEquatable<T>;
        public Task<ITaskResult> FindBookByISBNAsync(string isbn);
        public Task<ITaskResult> AddBookAsync(INewBook<BookDTO> newBookDto);
        public Task<ITaskResult> UpdateBookAsync(IBookDTO bookDto);
        public Task<ITaskResult> DeleteBookAsync<T>(T id) where T: IEquatable<T>;
    }
}
