using Microsoft.AspNetCore.Http;
using MyLibrary.Helpers;
using MyLibrary.Resources.Languages;
using MyLibrary.Services.Api;
using MyLibrary.Shared.Interfaces.IDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary.Services
{
    public class ValidationService : IValidationService
    {
        private readonly IBookService _bookService;

        public ValidationService(IBookService bookService)
        {
            _bookService = bookService;
        }
        // Login validations
        public IValidationResult ValidateDate(DateTime date)
        {
            throw new NotImplementedException();
        }
        public IValidationResult ValidateEmail(string email)
        {
            var errors = new List<string>();
            if(!new System.ComponentModel.DataAnnotations.EmailAddressAttribute().IsValid(email))
            {
                errors.Add(Strings.Validators_Email_Invalid);
            }

            return errors.Count == 0 ?
                new ValidationResult(isValid: true)
                : new ValidationResult(isValid: false, errors: errors);
        }
        public IValidationResult ValidatePassword(string password)
        {
            var errors = new List<string>();
            if (string.IsNullOrWhiteSpace(password))
            {
                errors.Add(Strings.Validators_Password_NullOrEmpty);
            }
            if (password.Length < 8)
            {
                errors.Add(Strings.Validators_Password_MinChars);
            }
            if (password.Length > 20)
            {
                errors.Add(Strings.Validators_Password_MaxChars);
            }
            if (!password.Any(char.IsUpper))
            {
                errors.Add(Strings.Validators_Password_OneUppercase);
            }
            if (!password.Any(char.IsLower))
            {
                errors.Add(Strings.Validators_Password_OneLowercase);
            }

            return errors.Count == 0 ?
                new ValidationResult(isValid: true)
                : new ValidationResult(isValid: false, errors: errors);
        }
        public IValidationResult ValidatePhoneNumber(string phoneNumber)
        {
            throw new NotImplementedException();
        }
        public IValidationResult ValidateUsername(string username)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(username))
            {
                errors.Add(Strings.Validators_Username_NullOrEmpty);
            }
            if (username.Length < 3)
            {
                errors.Add(Strings.Validators_Username_MinChars);
            }
            if (username.Length > 30)
            {
                errors.Add(Strings.Validators_Username_MaxChars);
            }
            if (!username.All(char.IsLetterOrDigit))
            {
                errors.Add(Strings.Validators_Username_LettersAndDigits);
            }

            return errors.Count == 0 ?
                new ValidationResult(isValid: true)
                : new ValidationResult(isValid: false, errors: errors);
        }

        // Sell validations
        public async Task<IValidationResult> ValidateOrder(IOrder order)
        {
            var errors = new List<string>();

            // orderItem =>
            // if order is a book then retrieve the book from db,
            // if order is not a book then treat it as an item. => retrieve as WarehouseDTO

            // Validate Null Order
            if (order == null)
            {
                errors.Add(Strings.Sell_Validations_NullOrder);
            }

            if(string.IsNullOrWhiteSpace(order!.ItemId))
            {
                errors.Add(Strings.Sell_Validations_ID_NullOrWhiteSpace);
            }

            if (string.IsNullOrWhiteSpace(order.ItemISBN))
            {
                errors.Add(Strings.Sell_Validations_ISBN_NullOrWhiteSpace);
            }

            if (order.Quantity <= 1)
            {
                errors.Add(Strings.Sell_Validations_NullQuantity);
            }

            var findByISBN = await _bookService.FindBookByISBNAsync(order.ItemISBN!);
            if (!findByISBN!.Succeeded)
            {
                errors.Add(Strings.Sell_Validations_ItemNotFound);
            }

            var findById = await _bookService.FindBookByIdAsync(order.ItemId!);
            if (!findById!.Succeeded)
            {
                errors.Add(Strings.Sell_Validations_ItemNotFound);
            }

            // Check for book availability and quantity.

            // Return
            return errors.Count == 0 ?
                new ValidationResult(isValid: true)
                : new ValidationResult(isValid: false, errors: errors);
        }
    }
}
