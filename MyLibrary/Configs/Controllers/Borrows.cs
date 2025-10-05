using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Configs.Controllers
{
    public class Borrows
    {
        public string GetAllBorrows { get; set; } = string.Empty;
        public string GetUserBorrows { get; set; } = string.Empty;
        public string GetBooksBorrows { get; set; } = string.Empty;
    }
}
