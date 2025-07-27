using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WerkzeugShared.DTO;
using WerkzeugShared.MVVM.Model;


namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class WerkzeugVerwaltung
    {
        private readonly WerkzeugMobilContext _context;

        public WerkzeugVerwaltung(WerkzeugMobilContext context)
        {
            _context = context;
        }

        // Asynchronous search method
        public async Task<List<Werkzeug>> SucheWerkzeugeAsync(string suchkriterium)
        {
            if (string.IsNullOrWhiteSpace(suchkriterium))
            {
                return new List<Werkzeug>(); // Return an empty list if the search criterion is empty
            }

            try
            {
                return await Task.Run(() =>
                    _context.Werkzeuge
                        .Where(w =>
                            w.Marke.Contains(suchkriterium, StringComparison.OrdinalIgnoreCase) ||
                            w.Art.Contains(suchkriterium, StringComparison.OrdinalIgnoreCase) ||
                            w.ProjektAdresse.Contains(suchkriterium, StringComparison.OrdinalIgnoreCase))
                        .ToList());
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error searching for tools: {ex.Message}");
                return new List<Werkzeug>(); // Return an empty list on error
            }
        }

       
        public async Task<List<ProjektDTO>> AlleProjekteAnzeigenAsync()
        {
            try
            {
                return await Task.Run(() =>
                    _context.Projekte.Select(p => new ProjektDTO
                    {
                        ProjektAddresse = p.ProjektAddresse.ToString(), // Assuming Id is an integer, convert it to string for the street name
                        Werkzeuge = p.Werkzeuge.Select(w => new WerkzeugDto
                        {
                            WerkzeugId = w.WerkzeugId,
                            Marke = w.Marke,
                            Art = w.Art,
                            ProjektAdresse = w.ProjektAdresse
                            // Map other properties as needed
                        }).ToList()
                    }).ToList());
            }
            catch (Exception ex)
            {
                // Handle exceptions (e.g., log the error)
                Console.WriteLine($"Error retrieving projects: {ex.Message}");
                return new List<ProjektDTO>(); // Return an empty list on error
            }
        }
    }
}

