using MyLibrary.Server.Data.DTOs;

namespace MyLibrary.Server.Http.Responses
{
    public class OperationTaskResult : ITaskResult
    {
        public string? Message { get; }

        public bool Succeeded { get; }

        public int StatusCode { get; }

        public OperationDTO? OperationDTO { get; set; }
        public ICollection<OperationDTO>? OperationDTOs { get; set; }

        public OperationTaskResult(bool succeeded, int statusCode, string? message = "", OperationDTO? operationDto = null, ICollection<OperationDTO>? operationDtos = null)
        {
            Succeeded = succeeded;
            StatusCode = statusCode;
            Message = message;
            OperationDTO = operationDto;
            OperationDTOs = operationDtos;
        }
    }
}
