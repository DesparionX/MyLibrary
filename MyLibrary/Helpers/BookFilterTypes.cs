using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Helpers
{
    public static class BookFilterTypes
    {
        public enum FilterType
        {
            Title,
            Author,
            Genre,
            ISBN
        }
    }
}
