using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WerkzeugShared.DTO;

namespace WerkzeugShared.MVVM.Model
{
    public class Werkzeug
    {

        public string WerkzeugId { get; set; } // Required
        public string Marke { get; set; }
        public string Art { get; set; }
        public string ProjektAdresse { get; set; }
        public string Beschreibung { get; set; }
        public bool Lager { get; set; }

        public List<string> History { get; set; }
        public ProjektDTO Projekt { get;  set; }

        public static implicit operator Werkzeug(WerkzeugDto v)
        {
            throw new NotImplementedException();
        }
    }



}
