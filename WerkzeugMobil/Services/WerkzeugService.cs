using System.Collections.Generic;
using System.Linq;
using WerkzeugMobil.MVVM.Model;

namespace WerkzeugMobil.Services
{
    public class WerkzeugServices
    {
        private readonly WerkzeugMobilContext _context;

        public WerkzeugServices(WerkzeugMobilContext context)
        {
            _context = context;
        }

        public List<Werkzeug> GetWerkzeugeByProjekt(string projektAddresse)
        {
            return _context.Werkzeuge.Where(w => w.ProjektAddresse == projektAddresse).ToList();
        }
    }
}