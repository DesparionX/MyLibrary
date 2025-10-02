using MyLibrary.ViewModels;
using System.Windows.Controls;

namespace MyLibrary.Views.Pages.Return
{
    /// <summary>
    /// Interaction logic for ReturnView.xaml
    /// </summary>
    public partial class ReturnView : UserControl
    {
        private readonly ReturnViewModel _viewModel;
        public ReturnView(ReturnViewModel viewModel)
        {
            _viewModel = viewModel;
            InitializeComponent();
            DataContext = _viewModel;
        }
    }
}
