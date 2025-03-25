namespace MyLibrary.Server.Http.Responses
{
    public interface ITaskResult
    {
        string? Message { get; }
        bool Succeeded { get; }
        int StatusCode { get; }
    }
}
