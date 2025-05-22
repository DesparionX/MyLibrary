using MyLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyLibrary.Views
{
    /// <summary>
    /// Interaction logic for LogIn.xaml
    /// </summary>
    public partial class LogIn : Window
    {
        private readonly LogInViewModel _viewModel;
        public LogIn(LogInViewModel vm)
        {
            _viewModel = vm;
            DataContext = vm;
            InitializeComponent();
        }

        private void Email_TextChanged(object sender, TextChangedEventArgs e)
        {
            _viewModel.Email = ((TextBox)sender).Text;
            _viewModel.CheckEmail(_viewModel.Email);
        }
        private void Email_LostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.Email = ((TextBox)sender).Text;
            _viewModel.CheckEmail(_viewModel.Email);
        }
        private void Password_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = ((PasswordBox)sender).Password;
            _viewModel.CheckPassword(_viewModel.Password);
        }
        private void Password_LostFocus(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = ((PasswordBox)sender).Password;
            _viewModel.CheckPassword(_viewModel.Password);
        }

        private async void LogInButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.LogInAsync();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            App.Current.Shutdown();
        }

        
    }
}
