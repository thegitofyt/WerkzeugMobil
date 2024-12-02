using System.Collections.Generic;
using System.Linq;
using WerkzeugMobil.MVVM.Model;

namespace WerkzeugMobil.Services
{
    public class BenutzerServices
    {
        private readonly WerkzeugMobilContext _context;

        public BenutzerServices(WerkzeugMobilContext context)
        {
            _context = context;
        }

        public Benutzer BenutzerLogin(string benutzername, string passwort)
        {
            return _context.Benutzer
                .FirstOrDefault(b => b.Benutzername == benutzername && b.Passwort == passwort);
        }
    }
}
