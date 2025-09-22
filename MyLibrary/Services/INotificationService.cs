using MyLibrary.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MyLibrary.Helpers.CustomDialogTypes;

namespace MyLibrary.Services
{
    public interface INotificationService
    {
        bool? ShowNotification(string title, string message);
        bool? ShowError(string title, string message);
        bool? ShowWarning(string title, string message);
        bool? ShowInfo(string title, string message);
        bool? ShowSuccess(string title, string message);
        bool? ShowConfirm(string title, string message);
    }
}
