using System;
using System.Collections.Generic;

namespace WerkzeugMobil.MVVM.Model
{
    public class Werkzeug
    {
        public int WerkzeugNummer { get; set; } // Required
        public string Marke { get; set; }
        public string Art { get; set; }
        public string ProjektAddress { get; set; }
        public string Beschreibung { get; set; }
        public bool Lager { get; set; }
    }



}
