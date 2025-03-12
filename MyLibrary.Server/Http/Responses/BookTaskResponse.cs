﻿using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Data.Entities;

namespace MyLibrary.Server.Http.Responses
{
    public class BookTaskResponse : ITaskResponse
    {
        public string? Message { get; }
        public bool Succeeded { get; }
        public int StatusCode { get; }
        public ICollection<IBookDTO>? Books { get; }
        public IBookDTO? Book { get; }

        public BookTaskResponse(bool succeeded, int statusCode, string? message = "", ICollection<IBookDTO>? books = null, IBookDTO? book = null)
        {
            Succeeded = succeeded;
            Message = message;
            StatusCode = statusCode;
            Books = books;
            Book = book;
        }
    }
}
