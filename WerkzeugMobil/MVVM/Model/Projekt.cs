using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerkzeugMobil.MVVM.Model
{
    public class Projekt
    {
        public string Address { get; set; } // Projekt ID / Address
        public List<Werkzeug> Werkzeuge { get; set; }
    }


}
