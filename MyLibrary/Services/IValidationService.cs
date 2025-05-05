using MyLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services
{
    public interface IValidationService
    {
        IValidationResult ValidateEmail(string email);
        IValidationResult ValidatePhoneNumber(string phoneNumber);
        IValidationResult ValidatePassword(string password);
        IValidationResult ValidateUsername(string username);
        IValidationResult ValidateDate(DateTime date);
    }
}
