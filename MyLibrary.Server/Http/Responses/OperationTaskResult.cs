namespace MyLibrary.Server.Http.Responses
{
    public class OperationTaskResult : ITaskResult
    {
        public string? Message { get; }

        public bool Succeeded { get; }

        public int StatusCode { get; }

        public OperationTaskResult(bool succeeded, int statusCode, string? message = "")
        {
            Succeeded = succeeded;
            StatusCode = statusCode;
            Message = message;
        }
    }
}
