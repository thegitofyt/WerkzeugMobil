using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Linq;
using System.Text;
using System.Windows;
using WerkzeugMobil.Data;
using WerkzeugMobil.Services;

namespace WerkzeugMobil
{
    public partial class App : Application
    {
        // Logger initialization with Serilog
        private static readonly ILogger<App> _logger = LoggerFactory.Create(builder =>
        {
            builder.AddSerilog();
        }).CreateLogger<App>();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Set up Serilog logging configuration
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console()
                .WriteTo.File("logs/startup.log", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            try
            {
                // Instantiate DbContext and LoggerFactory
                using var context = new WerkzeugDbContext();
                var loggerFactory = LoggerFactory.Create(builder => builder.AddSerilog());
                var logger = loggerFactory.CreateLogger<DataSeeder>();

                // Pass both logger and loggerFactory to DataSeeder
                var seeder = new DataSeeder(context, logger, loggerFactory);
                seeder.Seed();

                // Display User Info after seeding
                ShowUserInfo(context);
            }
            catch (Exception ex)
            {
                // Log error and display critical message
                _logger.LogError(ex, "Startup failed");
                MessageBox.Show($"Startup failed: {ex.Message}", "Critical Error", MessageBoxButton.OK, MessageBoxImage.Error);
                Environment.Exit(1);
            }
        }

        private void ShowUserInfo(WerkzeugDbContext context)
        {
            try
            {
                // Fetch users from the database
                var users = context.Benutzer.ToList();
                var sb = new StringBuilder("User details:\n");

                // Check if users exist
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

                // Show user information in MessageBox
                MessageBox.Show(sb.ToString(), "User Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch or display user info");
            }
        }
    }
}