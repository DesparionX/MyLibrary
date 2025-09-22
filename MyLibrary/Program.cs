using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyLibrary.Configs;
using MyLibrary.Services;
using MyLibrary.Services.Api;
using MyLibrary.ViewModels;
using MyLibrary.Views;
using MyLibrary.Views.Pages;
using MyLibrary.Views.Pages.Borrow;
using MyLibrary.Views.Pages.Return;
using MyLibrary.Views.Pages.Sell;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyLibrary
{
    public class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var app = new App();

            app.Resources.Add("Services", host.Services);
            app.InitializeComponent();
            var loadingScreen = host.Services.GetRequiredService<StartingLanguage>();

            app.Run(loadingScreen);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
                })
                .ConfigureServices((context, services) =>
                {
                    var configuration = context.Configuration;

                    // Add configuration settings
                    services.Configure<ApiSettings>(configuration.GetSection("ApiSettings"));
                    services.AddHttpClient("LibStore").AddHttpMessageHandler<AuthHeaderHandler>();
                    
                    // Services
                    services.AddSingleton<IAuthService, AuthService>();
                    services.AddSingleton<INavigationService, NavigationService>();
                    services.AddSingleton<INotificationService, NotificationService>();
                    services.AddSingleton<ILanguageService, LanguageService>();
                    services.AddSingleton<IFileService, FileService>();

                    // API Services
                    services.AddTransient<ApiService>();
                    services.AddTransient<IApiTestService, ApiTestService>();
                    services.AddTransient<AuthHeaderHandler>();
                    services.AddTransient<IUserService, UserService>();
                    services.AddTransient<IValidationService, ValidationService>();
                    services.AddTransient<IBookService, BookService>();
                    services.AddScoped<IOperationService, OperationService>();

                    // Windows
                    services.AddTransient<App>();
                    services.AddTransient<StartingLanguage>();
                    services.AddTransient<LoadingScreen>();
                    services.AddTransient<LogIn>();
                    services.AddTransient<MainWindow>();
                    services.AddScoped<AddItemToReceiptWindow>();

                    // Views
                    services.AddTransient<HomeView>();
                    services.AddTransient<NotFoundView>();
                    services.AddTransient<BorrowView>();
                    services.AddScoped<SellView>();
                    services.AddTransient<ReturnView>();

                    // View Models
                    services.AddTransient<ChangeLanguageViewModel>();
                    services.AddTransient<LoadingScreenViewModel>();
                    services.AddTransient<LogInViewModel>();
                    services.AddTransient<MainWindowViewModel>();
                    services.AddTransient<HomeViewModel>();
                    services.AddTransient<BorrowViewModel>();
                    services.AddScoped<SellViewModel>();
                    services.AddTransient<ReturnViewModel>();
                    services.AddScoped<AddItemToReceiptViewModel>();
                });
    }
}
