using Dotmim.Sync.SqlServer;
using Dotmim.Sync;
using Microsoft.Extensions.Logging;
using PdfSharpCore.Fonts;
using Serilog;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using WerkzeugMobil.Data;
using WerkzeugMobil.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace WerkzeugMobil
{
    public partial class App : Application
    {
        public static string ConnectionString { get; private set; }
        public static bool IsTemporaryLogin { get; set; } = false;

        private IHost _apiHost;

        private static readonly ILogger<App> _logger = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog();
        }).CreateLogger<App>();

        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            Application.Current.ShutdownMode = ShutdownMode.OnMainWindowClose;

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/startup.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            GlobalFontSettings.FontResolver = new CustomFontResolver();

            try
            {
                // ✅ Start the embedded ASP.NET Core Web API server (non-blocking)
                _apiHost = Host.CreateDefaultBuilder()
                    .ConfigureWebHostDefaults(webBuilder =>
                    {
                        webBuilder.UseStartup<Startup>();
                        webBuilder.UseUrls("http://localhost:5000"); // Match this in your frontend
                    })
                    .Build();

                await _apiHost.StartAsync();

                // ✅ Seed the database
                using var scope = _apiHost.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<WerkzeugDbContext>();
                var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog());
                var logger = loggerFactory.CreateLogger<DataSeeder>();
                var seeder = new DataSeeder(context, logger, loggerFactory);
                seeder.Seed();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Startup failed");
                MessageBox.Show($"Startup failed: {ex.Message}", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }

            // ✅ Show splash and login UI
            var login = new LoginUser();
            Application.Current.MainWindow = login;

            var miniSplash = new SmallLogoSplash();
            miniSplash.Show();
            await Task.Delay(1200);
            miniSplash.Close();

            var fullSplash = new FullLogoSplash();
            fullSplash.Show();
            await Task.Delay(2000);
            fullSplash.Close();

            login.Show();
            Debug.WriteLine($"Login shown. Is app running? {Application.Current.Windows.Count} windows open");
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            if (_apiHost != null)
            {
                await _apiHost.StopAsync(TimeSpan.FromSeconds(5));
                _apiHost.Dispose();
            }

            base.OnExit(e);
        }

        private void ShowUserInfo(WerkzeugDbContext context)
        {
            try
            {
                var users = context.Benutzer.ToList();
                var sb = new StringBuilder("User details:\n");

                if (users.Any())
                {
                    foreach (var user in users)
                    {
                        sb.AppendLine($"Username: {user.Benutzername}, Password: {user.Passwort}");
                    }
                }
                else
                {
                    sb.AppendLine("No users found.");
                }

                MessageBox.Show(sb.ToString(), "User Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch or display user info");
            }
        }
    }
}