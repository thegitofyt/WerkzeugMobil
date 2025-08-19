using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using System;
using System.IO;
using WerkzeugShared;
namespace WerkzeugMobil
{
    public class WerkzeugDbContextFactory : IDesignTimeDbContextFactory<WerkzeugDbContext>
    {
        public WerkzeugDbContext CreateDbContext(string[] args)
        {
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbDirectory = Path.Combine(localAppData, "WerkzeugMobil");

            if (!Directory.Exists(dbDirectory))
                Directory.CreateDirectory(dbDirectory);

            var dbPath = Path.Combine(dbDirectory, "WerkzeugMobilDb.sqlite");

            var optionsBuilder = new DbContextOptionsBuilder<WerkzeugDbContext>();
            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new WerkzeugDbContext(optionsBuilder.Options);
        }
    }
}