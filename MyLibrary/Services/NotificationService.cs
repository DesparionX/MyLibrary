using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace MyLibrary.Services.Api
{
    public class NotificationService : INotificationService
    {
        public void ShowError(string title, string message)
        {
            MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public void ShowInfo(string title, string message)
        {
            throw new NotImplementedException();
        }

        public void ShowNotification(string title, string message)
        {
            throw new NotImplementedException();
        }

        public void ShowSuccess(string title, string message)
        {
            throw new NotImplementedException();
        }

        public void ShowWarning(string title, string message)
        {
            throw new NotImplementedException();
        }
    }
}
