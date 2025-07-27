using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WerkzeugShared.DTO;
using WerkzeugShared.MVVM.Model;
using Microsoft.EntityFrameworkCore.Sqlite;


namespace WerkzeugShared.Services
{
    public class ProjektServices
    {

        private readonly List<Projekt> _projektList = new List<Projekt>();

        public static ProjektDTO SelectedProjekt { get; private set; }

        // Select a project
        public static void SetSelectedProjekt(ProjektDTO projekt)
        {
            SelectedProjekt = projekt;
        }
        private WerkzeugDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WerkzeugDbContext>();

            // Adjust path if needed - example using local app data folder
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = Path.Combine(localAppData, "WerkzeugMobil", "WerkzeugMobilDb.sqlite");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new WerkzeugDbContext(optionsBuilder.Options);
        }
        // Add a new Werkzeug to the list
        public void AddProjekt(ProjektDTO projektDto)
        {
            using (var context = CreateDbContext())
            {
                var existingProjekt = context.Projekte
                    .FirstOrDefault(p => p.ProjektAddresse == projektDto.ProjektAddresse); ;


                if (existingProjekt == null)
                {
                    // Create a new WerkzeugDto based on the Werkzeug entity
                    var projektDtoToAdd = new ProjektDTO
                    {
                        ProjektAddresse = projektDto.ProjektAddresse
                    };

                    // Add the new WerkzeugDto to the DbContext
                    context.Projekte.Add(projektDtoToAdd);
                    context.SaveChanges();  // Persist to the database
                }
                else
                {
                    // Update the existing werkzeug with the new WerkzeugDto data
                    existingProjekt.ProjektAddresse = projektDto.ProjektAddresse;


                    context.SaveChanges();  // Persist changes to the database
                }
            }
        }
    }
}
