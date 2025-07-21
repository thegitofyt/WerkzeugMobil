using System.Collections.Generic;
using System.Linq;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;


namespace WerkzeugMobil.Services
{
    public class WerkzeugServices
    {
        public Tools _tools;

        private readonly WerkzeugDbContext _context;
        private readonly List<Werkzeug> _werkzeugList = new List<Werkzeug>();

        public WerkzeugServices(WerkzeugDbContext context)
        {
            _context = context;
        }

        // Add a new Werkzeug to the list
        public void AddWerkzeug(WerkzeugDto werkzeugDto)
        {
            
                var existingWerkzeug = _context.Werkzeuge
                    .FirstOrDefault(w => w.WerkzeugId == werkzeugDto.WerkzeugId);

                if (existingWerkzeug == null)
                {

                    //string newId = GenerateNextWerkzeugId(werkzeugDto.Art);
                    // Create a new WerkzeugDto based on the Werkzeug entity
                    var werkzeugDtoToAdd = new WerkzeugDto
                    {
                        WerkzeugId = werkzeugDto.WerkzeugId,
                        Marke = werkzeugDto.Marke,
                        Art = werkzeugDto.Art,
                        ProjektAdresse = werkzeugDto.ProjektAdresse,
                        Beschreibung = werkzeugDto.Beschreibung
                    };

                // Add the new WerkzeugDto to the DbContext
                _context.Werkzeuge.Add(werkzeugDtoToAdd);
                _context.SaveChanges();  // Persist to the database
                }
                else
                {
                    // Update the existing werkzeug with the new WerkzeugDto data
                    existingWerkzeug.Marke = werkzeugDto.Marke;
                    existingWerkzeug.Art = werkzeugDto.Art;
                    existingWerkzeug.ProjektAdresse = werkzeugDto.ProjektAdresse;
                    existingWerkzeug.Beschreibung = werkzeugDto.Beschreibung;

                _context.SaveChanges();  // Persist changes to the database
                }
            }
        
        //private string GenerateNextWerkzeugId(string toolName)
        //{
        //    if (!_tools.ToolTypeCounts.ContainsKey(toolName))
        //    {
        //        _tools.ToolTypeCounts[toolName] = 1;
        //    }
        //    else
        //    {
        //        _tools.ToolTypeCounts[toolName]++;
        //    }
        //    return $"{toolName}-{_tools.ToolTypeCounts[toolName]}";  // Example: "Hammer-1", "Schaufel-2"
        //}

        // Update the address history for an existing Werkzeug
        public void UpdateAddressHistory(string werkzeugId, string newAddress)
        {
            var werkzeug = _werkzeugList.FirstOrDefault(w => w.WerkzeugId == werkzeugId);
            if (werkzeug != null)
            {
                // If the tool already has an address, we can add the new address to the history
                if (!string.IsNullOrEmpty(werkzeug.ProjektAdresse))
                {
                    if (werkzeug.History.Count >= 5)
                    {
                        werkzeug.History.RemoveAt(0); // Keep only 5 addresses in history
                    }
                    werkzeug.History.Add(newAddress);
                }
            }
        }

        // Method to get all Werkzeuge
        public List<Werkzeug> GetAllWerkzeuge()
        {
            return _werkzeugList;
        }

       
        // Method to get address history for a specific Werkzeug
        public List<string> GetAddressHistory(string werkzeugId)
        {
            var werkzeug = _werkzeugList.FirstOrDefault(w => w.WerkzeugId == werkzeugId);
            return werkzeug?.History ?? new List<string>();
        }
    }
}
