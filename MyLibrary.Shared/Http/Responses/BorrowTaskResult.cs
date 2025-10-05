using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.DTOs;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Shared.Http.Responses
{
    public class BorrowTaskResult : ITaskResult
    {
        public string? Message { get; set; }

        public bool Succeeded { get; set; }

        public int StatusCode { get; set; }

        public ICollection<BorrowDTO>? BorrowDtos { get; set; }

        public BorrowDTO? BorrowDto { get; set; }

        public BorrowTaskResult(bool succeeded, int statusCode, ICollection<BorrowDTO>? borrowDtos = default, BorrowDTO? borrowDto = default, string? message = default)
        {
            Message = message;
            Succeeded = succeeded;
            StatusCode = statusCode;
            BorrowDtos = borrowDtos;
            BorrowDto = borrowDto;
        }
    }
}
