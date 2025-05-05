using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MyLibrary.Configs;
using MyLibrary.Services;
using MyLibrary.Services.Api;
using MyLibrary.ViewModels;
using MyLibrary.Views;
using System;
using System.Collections.Generic;
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
            var loadingScreen = host.Services.GetRequiredService<LoadingScreen>();

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
                    
                    // API Services
                    services.AddTransient<IApiTestService, ApiTestService>();
                    services.AddTransient<AuthHeaderHandler>();
                    services.AddTransient<IUserService, UserService>();
                    services.AddTransient<IValidationService, ValidationService>();

                    // Views
                    services.AddTransient<App>();
                    services.AddTransient<LoadingScreen>();
                    services.AddTransient<LogIn>();
                    services.AddTransient<MainWindow>();

                    // View Models
                    services.AddTransient<LoadingScreenViewModel>();
                    services.AddTransient<LogInViewModel>();
                    services.AddTransient<MainWindowViewModel>();
                });
    }
}
