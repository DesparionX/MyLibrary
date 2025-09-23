using MyLibrary.ViewModels;
using System.Windows;
using System.Windows.Controls;

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
