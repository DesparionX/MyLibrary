using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace MyLibrary.Services
{
    public interface INavigationService : INotifyPropertyChanged
    {
        public object CurrentView { get; set; }
        void NavigateTo<TWindow>() where TWindow : Window;
        void NavigateToView(string viewName);
        void BackToHomeView();
        void CloseCurrentWindow();
        void CloseApp();
    }
}
