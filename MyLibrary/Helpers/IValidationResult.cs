using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Helpers
{
    public interface IValidationResult
    {
        bool IsValid { get; }
        ICollection<string>? Errors { get; }
    }
}
