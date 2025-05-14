using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using System;
using System.Collections.Generic;

using System.IO;
using System.Linq;
using System.Text;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;
using WerkzeugMobil.Helpers;
using WerkzeugMobil.MVVM.Model;

namespace WerkzeugMobil.Services
{
    public class DataSeeder
    {
        private readonly WerkzeugDbContext _context;
        private readonly ILogger<DataSeeder> _logger;
        private readonly ILoggerFactory _loggerFactory;


        public DataSeeder(WerkzeugDbContext context, ILogger<DataSeeder> logger, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

        public void Seed()
        {
            _context.Database.Migrate();

            try
            {
                SeedProjekte();
                SeedUsers();
                SeedWerkzeuge(); // 👈 Use this instead of Excel
            }
            catch (Exception ex)
            {
                _logger.LogError("DataSeeder failed", ex);
                throw;
            }
        }

        private void SeedProjekte()
        {
            if (_context.Projekte.Any()) return;

            _context.Projekte.AddRange(new[]
            {
                new ProjektDTO { ProjektAddresse = "Strassergasse 34" },
                new ProjektDTO { ProjektAddresse = "Gentzgasse" },
                new ProjektDTO { ProjektAddresse = "Rosenhügelstrasse" },
                new ProjektDTO { ProjektAddresse = "Obere Donau Strasse 63" }
            });

            _context.SaveChanges();
            _logger.LogInformation("Projekte seeded successfully.");
        }

        private void SeedUsers()
        {
            if (_context.Benutzer.Any()) return;

            var users = new[] { "admin1", "admin2", "admin3" }
                .Select(name => new UserDTO
                {
                    Benutzername = name,
                    Passwort = PasswordGenerator.Generate(12)
                }).ToList();

            _context.Benutzer.AddRange(users);
            _context.SaveChanges();

           
        }

        private void SeedWerkzeuge()
        {
            // Create a logger specifically for WerkzeugSeeder
            var werkzeugSeederLogger = _loggerFactory.CreateLogger<WerkzeugSeeder>();

            // Pass the logger into WerkzeugSeeder constructor
            var werkzeugSeeder = new WerkzeugSeeder(_context, werkzeugSeederLogger);

            // Perform seeding
            werkzeugSeeder.SeedWerkzeugeAndTools();
        }
    }
}