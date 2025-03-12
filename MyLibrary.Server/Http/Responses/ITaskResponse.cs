namespace MyLibrary.Server.Http.Responses
{
    public interface ITaskResponse
    {
        string? Message { get; }
        bool Succeeded { get; }
        int StatusCode { get; }
    }
}
