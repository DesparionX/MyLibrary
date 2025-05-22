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
    /// Interaction logic for StartingLanguage.xaml
    /// </summary>
    public partial class StartingLanguage : Window
    {
        private readonly ChangeLanguageViewModel _viewModel;
        public StartingLanguage(ChangeLanguageViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
        }

        private void English_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ChangeLanguage("en");
        }

        private void Bulgarian_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.ChangeLanguage("bg");
        }
    }
}
