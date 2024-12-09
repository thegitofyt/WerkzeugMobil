using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WerkzeugMobil.MVVM.Model
{
    public class Werkzeug
    {
        [Key]
        public string WerkzeugNummer { get; set; } // Required
        public string Marke { get; set; }
        public string Art { get; set; }
        public string ProjektAdresse { get; set; }
        public string Beschreibung { get; set; }
        public bool Lager { get; set; }
    }



}
