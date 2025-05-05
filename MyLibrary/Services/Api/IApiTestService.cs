using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services.Api
{
    public interface IApiTestService
    {
        Task<bool> IsApiOnline();
        Task<bool> IsDbOnline();

    }
}
