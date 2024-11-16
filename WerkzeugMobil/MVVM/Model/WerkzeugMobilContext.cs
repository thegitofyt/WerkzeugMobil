using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;

namespace WerkzeugMobil.MVVM.Model
{
    public partial class WerkzeugMobilContext : DbContext
    {
        public WerkzeugMobilContext()
        {
        }
        public WerkzeugMobilContext(DbContextOptions options)
            : base(options)
        {
        }
        public virtual DbSet<Benutzer> Benutzer { get; set; }
        public virtual DbSet<Straße> Straße { get; set; }
        public virtual DbSet<Werkzeug> Werkzeug { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.LogTo(Console.WriteLine, LogLevel.Information)
                    .UseSqlite("DataSource=techshop.db");
                //optionsBuilder.UseMySQL("server=127.0.0.1;uid=root;pwd=root;database=techshop");
            }

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Benutzer>(entity =>
            {
                entity.ToTable("Benutzer");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.isAdmin).HasColumnName("isAdmin");
            });

            modelBuilder.Entity<Straße>(entity =>
            {
                entity.ToTable("Straße");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.Name).HasColumnName("Name");

                entity.HasOne(d => d.Werkzeug)
                    .WithMany(p => p.Straße)
                    .HasForeignKey(d => d.ID)
                    .HasConstraintName("FK_Straße_Werkzeug");
            });

            modelBuilder.Entity<Werkzeug>(entity =>
            {
                entity.ToTable("Werkzeug");

                entity.Property(e => e.ID).HasColumnName("ID");

                entity.Property(e => e.Art).HasColumnName("Art");


                entity.Property(e => e.Bemerkung).HasColumnName("Bemerkung");





                entity.Property(e => e.Marke).HasColumnName("Marke");


                entity.Property(e => e.Name).HasColumnName("Name");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        public void Seed()
        {
            Benutzer benutzer = new Benutzer();
            benutzer.ID = 1;
            benutzer.isAdmin = true;
            Benutzer.Add(benutzer);

            Straße straße = new Straße();
            straße.ID = 1;
            straße.Name = "Straße 1";
            Straße.Add(straße);

            Werkzeug werkzeug = new Werkzeug();
            werkzeug.ID = 1;
            werkzeug.Name = "Werkzeug 1";
            werkzeug.Marke = "Marke 1";
            werkzeug.Art = "Art 1";
            werkzeug.Bemerkung = "Bemerkung 1";
            
            werkzeug.Straße = straße;
            Werkzeug.Add(werkzeug);

            SaveChanges();
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
