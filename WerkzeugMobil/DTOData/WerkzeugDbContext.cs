using Microsoft.EntityFrameworkCore;
using WerkzeugMobil.DTO;

namespace WerkzeugMobil.Data
{
    public class WerkzeugDbContext : DbContext
    {
        

        public DbSet<WerkzeugDto> Werkzeuge { get; set; }
        public DbSet<ProjektDTO> Projekte { get; set; }
        public DbSet<UserDTO> Benutzer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=WerkzeugMobilDb.sqlite;").EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information); ;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<WerkzeugDto>()
                .HasOne(w => w.Projekt)
                .WithMany(p => p.Werkzeuge)
                .HasForeignKey(w => w.ProjektAdresse);
        }
    }
    
}