using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WerkzeugApi;
using WerkzeugShared;

public class Program
{
    public static void Main(string[] args)
    {
        var host = CreateHostBuilder(args).Build();

        // Ensure the database is created when the application starts
        using (var scope = host.Services.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<WerkzeugDbContext>();
            db.Database.EnsureCreated();
        }

        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                // Force Kestrel to listen on the port Azure expects (8080)
                var port = Environment.GetEnvironmentVariable("PORT") ?? "8080";
                webBuilder.UseStartup<Startup>()
                          .UseUrls($"http://*:{port}");
            });
}