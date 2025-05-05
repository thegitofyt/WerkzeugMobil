using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerkzeugMobil.DTO
{
    public class ProjektDTO
    {
        [Key]
        public string ProjektAddresse { get; set; } // Street name as the ID
        public List<WerkzeugDto>? Werkzeuge { get; set; } // List of tools in the project/street

        public ProjektDTO()
        {

            Werkzeuge = new List<WerkzeugDto>(); // Initialize the list
        }
    }
}
