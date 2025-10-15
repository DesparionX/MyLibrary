using MyLibrary.Server.Http.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.Api
{
    public interface IWarehouseService
    {
        Task<ITaskResult> GetStockAsync(string isbn);
    }
}
