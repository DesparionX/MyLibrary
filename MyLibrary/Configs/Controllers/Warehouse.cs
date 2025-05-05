using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Configs.Controllers
{
    public class Warehouse
    {
        public string GetStock { get; set; } = string.Empty;
        public string GetAllStocks { get; set; } = string.Empty;
        public string CreateStock { get; set; } = string.Empty;
        public string AddStock { get; set; } = string.Empty;
        public string UpdateStock { get; set; } = string.Empty;
        public string DeleteStock { get; set; } = string.Empty;
        public string RemoveStock { get; set; } = string.Empty;
    }
}
