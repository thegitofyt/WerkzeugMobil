using Microsoft.EntityFrameworkCore;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;

namespace WerkzeugMobil.Data
{
    public class WerkzeugDbContext : DbContext
    {
        

        public DbSet<WerkzeugDto> Werkzeuge { get; set; }
        public DbSet<ProjektDTO> Projekte { get; set; }
        public DbSet<UserDTO> Benutzer { get; set; }

        public DbSet<ToolDTO> Tools { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(@"Data Source=WerkzeugMobilDb.sqlite;").EnableSensitiveDataLogging()
        .LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information); 
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configure relationships
            modelBuilder.Entity<WerkzeugDto>()
                .HasOne(w => w.Projekt)
                .WithMany(p => p.Werkzeuge)
                .HasForeignKey(w => w.ProjektAdresse);

            modelBuilder.Entity<ToolDTO>()
            .Ignore(t => t.ToolTypeCounts) // Ignore the List<Tuple<string, int>>
            .Property(t => t.ToolTypeCountsSerialized)
            .HasColumnName("ToolTypeCounts"); // Use this column instead
        }
    }
    
}