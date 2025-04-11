using MyLibrary.Server.Data.DTOs.Interfaces;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public interface IOperationHandler
    {
        public Task<ITaskResult> PerformOperationAsync(IOperationDTO operationDTO);
        public Task<ITaskResult> GetOperationHistoryAsync();
    }
}
