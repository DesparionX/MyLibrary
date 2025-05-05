using MyLibrary.Shared.Interfaces.IDTOs;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers.Interfaces
{
    public interface IOperationHandler
    {
        public Task<ITaskResult> PerformOperationAsync(IOperationDTO operationDTO);
        public Task<ITaskResult> GetOperationHistoryAsync();
    }
}
