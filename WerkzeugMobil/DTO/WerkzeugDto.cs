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
       
        public ProjektDTO Projekt { get; internal set; }
    }
}
