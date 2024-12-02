using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WerkzeugMobil.MVVM.Model;


namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class WerkzeugVerwaltung
    {
        private readonly WerkzeugMobilContext _context;

        public WerkzeugVerwaltung(WerkzeugMobilContext context)
        {
            _context = context;
        }

        public List<Werkzeug> SucheWerkzeuge(string suchkriterium)
        {
            return _context.Werkzeuge
                .Where(w => 
                            w.Marke.Contains(suchkriterium) ||
                            w.Art.Contains(suchkriterium) ||
                            w.ProjektAddresse.Contains(suchkriterium))
                .ToList();
        }

        public List<Projekt> AlleProjekteAnzeigen()
        {
            return _context.Projekte;
        }
    }
}
