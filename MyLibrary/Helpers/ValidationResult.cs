using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Helpers
{
    public class ValidationResult : IValidationResult
    {
        public bool IsValid { get; }

        public ICollection<string>? Errors { get; } = new List<string>();

        public ValidationResult(bool isValid, List<string>? errors = null)
        {
            IsValid = isValid;
            if (errors != null)
            {
                foreach (var error in errors)
                {
                    Errors!.Add(error);
                }
            }
        }
    }
}
