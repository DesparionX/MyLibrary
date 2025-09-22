using MyLibrary.Resources.Languages;
using MyLibrary.ViewModels;
using MyLibrary.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using static MyLibrary.Helpers.CustomDialogTypes;

namespace MyLibrary.Services.Api
{
    public class NotificationService : INotificationService
    {
        public bool? ShowError(string title, string message)
        {
            return CreateDialog(type: DialogType.Error, title, message, confirmButtonText: Strings.CustomDialog_OK);
        }

        public bool? ShowInfo(string title, string message)
        {
            return CreateDialog(type: DialogType.Info, title, message, confirmButtonText: Strings.CustomDialog_OK);
        }

        public bool? ShowNotification(string title, string message)
        {
            return CreateDialog(type: DialogType.Info, title, message, confirmButtonText: Strings.CustomDialog_OK);
        }

        public bool? ShowSuccess(string title, string message)
        {
            return CreateDialog(type: DialogType.Success, title, message, confirmButtonText: Strings.CustomDialog_OK);
        }

        public bool? ShowWarning(string title, string message)
        {
            return CreateDialog(type: DialogType.Warning, title, message, confirmButtonText: Strings.CustomDialog_OK);
        }
        public bool? ShowConfirm(string title, string message)
        {
            return CreateDialog(type: DialogType.Error, title, message, confirmButtonText: Strings.CustomDialog_Confirm, cancelButtonText: Strings.CustomDialog_Cancel);
        }

        private static bool? CreateDialog(DialogType type, string title, string message, string? confirmButtonText = default, string? cancelButtonText = default)
        {
            var dialog = new CustomDialogViewModel(type, title, message, confirmButtonText, cancelButtonText);
            var customDialog = new CustomDialog(dialog);

            return customDialog.ShowDialog();
        }
    }
}
