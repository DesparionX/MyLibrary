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
    /// Interaction logic for LoadingScreen.xaml
    /// </summary>
    public partial class LoadingScreen : Window
    {
        private readonly LoadingScreenViewModel _viewModel;
        public LoadingScreen(LoadingScreenViewModel vm)
        {
            DataContext = vm;
            _viewModel = vm;
            
            InitializeComponent();
            Loaded += SplashScreen_Loaded;

        }

        private async void SplashScreen_Loaded(object sender, RoutedEventArgs e)
        {
            await _viewModel.PerformHealthChecksAsync();
        }
    }
}
