using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Configs.Controllers
{
    public class Books
    {
        public string FindByISBN { get; set; } = string.Empty;
        public string GetAllBooks { get; set; } = string.Empty;
        public string AddBook { get; set; } = string.Empty;
        public string DeleteBook { get; set; } = string.Empty;
        public string UpdateBook { get; set; } = string.Empty;
    }
}
