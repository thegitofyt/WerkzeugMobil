using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using WerkzeugShared.DTO;
using WerkzeugShared;

namespace WerkzeugApi.Seeder
{
    public class DataSeeder
    {
        private readonly WerkzeugShared.WerkzeugDbContext _context;
        private readonly ILogger<DataSeeder> _logger;
        private readonly ILoggerFactory _loggerFactory;

        public DataSeeder(WerkzeugShared.WerkzeugDbContext context, ILogger<DataSeeder> logger, ILoggerFactory loggerFactory)
        {
            _context = context;
            _logger = logger;
            _loggerFactory = loggerFactory;
        }

        public void Seed()
        {
            // Only apply migrations — do NOT recreate or drop database!
            _context.Database.Migrate();

            try
            {
                SeedProjekte();
                SeedUsers();
                SeedWerkzeuge(); // Call external seeder
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ DataSeeder failed during execution.");
                throw;
            }
        }

        private void SeedProjekte()
        {
            if (_context.Projekte.Any())
            {
                _logger.LogInformation("ℹ️ Projekte table already contains data. Skipping seeding.");
                return;
            }

            _context.Projekte.AddRange(new[]
            {
                new ProjektDTO { ProjektAddresse = "Strassergasse 34" },
                new ProjektDTO { ProjektAddresse = "Gentzgasse" },
                new ProjektDTO { ProjektAddresse = "Rosenhügelstrasse" },
                new ProjektDTO { ProjektAddresse = "Obere Donau Strasse 63" }
            });

            _context.SaveChanges();
            _logger.LogInformation("✅ Projekte seeded successfully.");
        }

        private void SeedUsers()
        {
            var usersToUpdate = _context.Benutzer.ToList();

            if (!usersToUpdate.Any())
            {
                var newUsers = new List<UserDTO>
                {
                    new UserDTO { Benutzername = "user1", Passwort = BCrypt.Net.BCrypt.HashPassword("password1") },
                    new UserDTO { Benutzername = "user2", Passwort = BCrypt.Net.BCrypt.HashPassword("password2") },
                    new UserDTO { Benutzername = "user3", Passwort = BCrypt.Net.BCrypt.HashPassword("password3") }
                };

                _context.Benutzer.AddRange(newUsers);
                _context.SaveChanges();
                _logger.LogInformation("✅ Benutzer (users) seeded successfully.");
                return;
            }

            foreach (var user in usersToUpdate)
            {
                if (!(user.Passwort.StartsWith("$2a$") || user.Passwort.StartsWith("$2b$") || user.Passwort.StartsWith("$2y$")))
                {
                    user.Passwort = BCrypt.Net.BCrypt.HashPassword(user.Passwort);
                    _logger.LogWarning($"⚠️ Password for user '{user.Benutzername}' was plaintext. Rehashed.");
                }
            }

            _context.SaveChanges();
            _logger.LogInformation("✅ Benutzer passwords checked and updated if necessary.");
        }

        private void SeedWerkzeuge()
        {
            var werkzeugSeederLogger = _loggerFactory.CreateLogger<WerkzeugSeeder>();
            var werkzeugSeeder = new WerkzeugSeeder(_context, werkzeugSeederLogger);

            werkzeugSeeder.SeedWerkzeugeAndTools();
            _logger.LogInformation("✅ Werkzeuge seeding completed.");
        }
    }
}