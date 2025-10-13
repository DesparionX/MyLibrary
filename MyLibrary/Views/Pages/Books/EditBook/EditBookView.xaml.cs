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

namespace MyLibrary.Views.Pages.Books.EditBook
{
    public partial class EditBookView : Window
    {
        private readonly EditBookViewModel _viewModel;

        public EditBookView(EditBookViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
            DataContext = _viewModel;
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            DialogResult = false;

            Close();
        }

        private async void SaveChanges(object sender, RoutedEventArgs e)
        {
            if (!await _viewModel.SaveChangesAsync()) return;

            DialogResult = true;

            Close();
        }
    }
}
