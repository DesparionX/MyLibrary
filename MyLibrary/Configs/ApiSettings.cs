using MyLibrary.Configs.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Configs
{
    public class ApiSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public Controllers.Controllers Controllers { get; set; } = new();
    }
}
