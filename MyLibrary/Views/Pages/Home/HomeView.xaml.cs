using MyLibrary.ViewModels;
using System.Windows.Controls;

namespace MyLibrary.Views.Pages
{
    /// <summary>
    /// Interaction logic for HomeView.xaml
    /// </summary>
    public partial class HomeView : UserControl
    {
        private readonly HomeViewModel _viewModel;
        public HomeView(HomeViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
            
        }
    }
}
