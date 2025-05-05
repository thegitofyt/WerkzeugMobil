using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;

namespace WerkzeugMobil.Services
{
    public class ProjektServices
    {

        private readonly List<Projekt> _projektList = new List<Projekt>();

        // Add a new Werkzeug to the list
        public void AddProjekt(ProjektDTO projektDto)
        {
            using (var context = new WerkzeugDbContext())
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
