using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerkzeugMobil.MVVM.Model
{
    public class Tools
    {
        public List <Werkzeug> ListeWerkzeuge { get; set; }
        public Dictionary <string, Werkzeug> keyValuePairs { get; set; }
    }
}
