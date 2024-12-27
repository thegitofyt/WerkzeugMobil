using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WerkzeugMobil.DTO;

namespace WerkzeugMobil.MVVM.Model
{
    public class Projekt
    {
        public string Id { get; set; } // Street name as the ID
        public List<WerkzeugDto> Werkzeuge { get; set; } // List of tools in the project/street
    }


}
