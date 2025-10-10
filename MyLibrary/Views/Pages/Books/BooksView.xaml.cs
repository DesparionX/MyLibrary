using MyLibrary.ViewModels;
using System.Windows;
using System.Windows.Controls;

namespace MyLibrary.Views.Pages.Books
{
    /// <summary>
    /// Interaction logic for BooksView.xaml
    /// </summary>
    public partial class BooksView : UserControl
    {
        private readonly BookViewModel _viewModel;
        public BooksView(BookViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
            DataContext = _viewModel;

        }

        private async void LoadBooks(object sender, EventArgs e)
        {
            await _viewModel.LoadBooksAsync();
        }
    }
}
