using Microsoft.AspNetCore.Mvc;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public interface IResultHandler<TTaskResponse> where TTaskResponse : class, ITaskResult
    {
        IActionResult ReadResult(TTaskResponse response);
    }
}
