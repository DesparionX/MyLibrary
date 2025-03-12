﻿using MyLibrary.Server.Data.DTOs;

namespace MyLibrary.Server.Http.Responses
{
    public class WarehouseTaskResponse : ITaskResponse
    {
        public string? Message { get; }

        public bool Succeeded { get; }

        public int StatusCode { get; }
        public IWarehouseDTO? Warehouse { get; }

        public WarehouseTaskResponse(bool succeeded, int statusCode, string? message = "", IWarehouseDTO? warehouseDto = null)
        {
            Succeeded = succeeded;
            StatusCode = statusCode;
            Message = message;
            Warehouse = warehouseDto;
        }
    }
}
