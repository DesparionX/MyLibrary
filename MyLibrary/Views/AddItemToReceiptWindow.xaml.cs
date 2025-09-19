using Microsoft.Extensions.DependencyInjection;
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
    public partial class AddItemToReceiptWindow : Window
    {
        private readonly AddItemToReceiptViewModel _viewModel;

        public AddItemToReceiptWindow(AddItemToReceiptViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
            DataContext = _viewModel;
            _viewModel.Dialog = this;
            ISBN.Focus();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.Cancel();
        }
        private async void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            await _viewModel.Confirm();
        }
        private void Validate_ISBN(object sender, RoutedEventArgs e)
        {
            _viewModel.Isbn = (sender as TextBox)!.Text;
            _viewModel.ValidateISBN();
        }

    }
}
