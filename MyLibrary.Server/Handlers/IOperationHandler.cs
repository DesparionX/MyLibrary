using MyLibrary.Server.Data.DTOs;
using MyLibrary.Server.Http.Responses;

namespace MyLibrary.Server.Handlers
{
    public interface IOperationHandler
    {
        public Task<ITaskResponse> SellItemAsync(IOperationDTO operationDTO);
        public Task<ITaskResponse> SellItemAsync<TId>(IOperationDTO<TId> operationDTO) where TId : IEquatable<TId>;

        public Task<ITaskResponse> BorrowItemAsync(IOperationDTO operationDTO);
        public Task<ITaskResponse> BorrowItemAsync<TId>(IOperationDTO<TId> operationDTO) where TId : IEquatable<TId>;

        public Task<ITaskResponse> ReturnItemAsync(IOperationDTO operationDTO);
        public Task<ITaskResponse> ReturnItemAsync<TId>(IOperationDTO<TId> operationDTO) where TId : IEquatable<TId>;
    }
}
