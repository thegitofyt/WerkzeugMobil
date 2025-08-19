using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System.IO;
using System.Text.Json;
using WerkzeugShared.DTO;


namespace WerkzeugShared
{
    public class WerkzeugDbContext : DbContext
    {
        public WerkzeugDbContext(DbContextOptions<WerkzeugDbContext> options)
            : base(options)
        {
        }

        public DbSet<WerkzeugDto> Werkzeuge { get; set; }
        public DbSet<ProjektDTO> Projekte { get; set; }
        public DbSet<UserDTO> Benutzer { get; set; }
        public DbSet<ToolDTO> Tools { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WerkzeugDto>()
                .HasOne(w => w.Projekt)
                .WithMany(p => p.Werkzeuge)
                .HasForeignKey(w => w.ProjektAdresse);

            var historyComparer = new ValueComparer<List<string>>(
                (c1, c2) => c1.SequenceEqual(c2),
                c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
                c => c.ToList());

            modelBuilder.Entity<WerkzeugDto>()
                .Property(w => w.History)
                .HasConversion(
                    v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null),
                    v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null))
                .Metadata.SetValueComparer(historyComparer);

            modelBuilder.Entity<ToolDTO>()
                .Ignore(t => t.ToolTypeCounts)
                .Property(t => t.ToolTypeCountsSerialized)
                .HasColumnName("ToolTypeCounts");
        }
    }
}