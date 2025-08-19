using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.IO;
using System;
using WerkzeugShared;

namespace WerkzeugApi
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            var dbDirectory = Path.Combine("/home", "WerkzeugMobil");

            if (!Directory.Exists(dbDirectory))
                Directory.CreateDirectory(dbDirectory);

            var dbPath = Path.Combine(dbDirectory, "WerkzeugMobilDb.sqlite");

            services.AddDbContext<WerkzeugDbContext>(options =>
                options.UseSqlite($"Data Source={dbPath}"));

            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            services.AddCors(options =>
            {
                options.AddPolicy("AllowLocalhost",
                    builder => builder
                        .WithOrigins("http://localhost:5000", "https://localhost:5000")
                        .AllowAnyHeader()
                        .AllowAnyMethod());
            });

            services.AddEndpointsApiExplorer();

            // ✅ WICHTIG: AddSwaggerGen konfigurieren
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "WerkzeugMobil API",
                    Version = "v1",
                    Description = "API für Werkzeugverwaltung"
                });

                // Optional: XML-Kommentare (falls vorhanden)
                var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
            });
        }

        public void Configure(IApplicationBuilder app, IHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseHttpsRedirection();
            }

            // 🔹 Stelle sicher, dass die DB-Migrationen angewendet werden
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<WerkzeugDbContext>();
                db.Database.Migrate();
            }

            app.UseRouting();
            app.UseCors("AllowAll");
            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "WerkzeugMobil API v1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

    }
}