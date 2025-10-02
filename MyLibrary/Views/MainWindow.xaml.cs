using MaterialDesignThemes.Wpf;
using MyLibrary.ViewModels;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;

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
        
        private void SetUserInfo()
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
