using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Configs.Controllers
{
    public class HealthChecks
    {
        public string ApiStatus { get; set; } = string.Empty;
        public string ApiUI { get; set; } = string.Empty;
        public string UI { get; set; } = string.Empty;
    }
}
