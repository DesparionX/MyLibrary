using MyLibrary.Helpers;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services
{
    public interface IValidationService
    {
        // Login validations
        IValidationResult ValidateEmail(string email);
        IValidationResult ValidatePhoneNumber(string phoneNumber);
        IValidationResult ValidatePassword(string password);
        IValidationResult ValidateUsername(string username);
        IValidationResult ValidateDate(DateTime date);

        // Sell validations
        Task<IValidationResult> ValidateOrder(IOrder order);
    }
}
