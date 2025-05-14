using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WerkzeugMobil.DTO
{
    public class WerkzeugDto
    {

        [Key]
        public string WerkzeugId { get; set; }
        public string? Marke { get; set; }
        public string Art { get; set; }
        public string? Beschreibung { get; set; }
        public string? ProjektAdresse { get; set; }
        public List<string>? History { get; set; }

        public bool Lager { get; set; }
       
        public ProjektDTO? Projekt { get; internal set; }
        // Method to set default values
        public WerkzeugDto()
        {
            // If History is null or empty, set it to the default value
            if (History == null )
            {
                History = new List<string> { "Keine Adressen" };
            }

            // If Marke is null, set it to the default value
            if (string.IsNullOrEmpty(Marke))
            {
                Marke = "nicht Angegeben";
            }

            // If Beschreibung is null, set it to the default value
            if (string.IsNullOrEmpty(Beschreibung))
            {
                Beschreibung = string.Empty;
            }

            // If ProjektAdresse is null, set it to the default value
            if (string.IsNullOrEmpty(ProjektAdresse))
            {
                ProjektAdresse = "Im Lager";
            }
        }

    }
}
