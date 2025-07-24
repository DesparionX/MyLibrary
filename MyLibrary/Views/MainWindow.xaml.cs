using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using MyLibrary.Server.Data.DTOs;
using MyLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MyLibrary.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowViewModel _viewModel;

        public MainWindow(MainWindowViewModel viewModel)
        {
            _viewModel = viewModel;
            DataContext = _viewModel;
            InitializeComponent();
            SetUserInfo();
        }
        
        private async Task SetUserInfo()
        {
            TopBar.UserName = $"{_viewModel.User.FirstName} {_viewModel.User.LastName}";
            if (string.IsNullOrWhiteSpace(_viewModel.User.Avatar))
            {
                TopBar.Avatar.Child = new PackIcon
                {
                    Kind = PackIconKind.AccountCircle,
                    Width = Double.NaN,
                    Height = Double.NaN
                };
            }
            else
            {
                TopBar.Avatar.Background = new ImageBrush
                {
                    ImageSource = new BitmapImage(new Uri(_viewModel.GetUserAvatar())),
                    Stretch = Stretch.Fill
                };
            }
        }

        private void DrawerHost_DrawerClosing(object sender, DrawerClosingEventArgs e)
        {
            AnimateContentMargin(new Thickness(0, 0, 0, 0), 200);
            
        }

        private void DrawerHost_DrawerOpened(object sender, DrawerOpenedEventArgs e)
        {
            AnimateContentMargin(new Thickness(MainNav.ActualWidth, 0, 0, 0), 500);
        }

        // Add animation to margin change.
        private void AnimateContentMargin(Thickness toMargin, double durationMs)
        {
            var animation = new ThicknessAnimation
            {
                To = toMargin,
                Duration = TimeSpan.FromMilliseconds(durationMs),
                EasingFunction = new CubicEase { EasingMode = EasingMode.EaseInOut }
            };

            ContentView.BeginAnimation(FrameworkElement.MarginProperty, animation);
        }
    }
}
