using MyLibrary.Configs.Controllers.Admin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Configs.Controllers
{
    public class Controllers
    {
        public Account Account { get; set; } = new();
        public Books Books { get; set; } = new();
        public Operations Operations { get; set; } = new();
        public Warehouse Warehouse { get; set; } = new();
        public Admin.Admin Admin { get; set; } = new();
        public HealthChecks HealthChecks { get; set; } = new();
    }
}
