using MyLibrary.Server.Http.Responses;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.Api
{
    public interface IOperationService
    {
        public Task<ITaskResult?> GetOperationHistoryAsync();
        public Task<ITaskResult?> PerformOperation(IOperationDTO? operationDto);
    }
}
