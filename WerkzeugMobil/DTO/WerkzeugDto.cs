using System.Collections.Generic;

namespace WerkzeugMobil.DTO
{
    public class WerkzeugDto
    {
       
        public string WerkzeugId { get; set; }
        public string Marke { get; set; }
        public string Art { get; set; }
        public string Beschreibung { get; set; }
        public string ProjektAdresse { get; set; }
        public List<string> History { get; set; }
    }
}
