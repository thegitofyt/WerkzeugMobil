using System.Collections.Generic;
using System.Linq;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;

namespace WerkzeugMobil.Services
{
    public class WerkzeugServices
    {
        private readonly List<Werkzeug> _werkzeugList = new List<Werkzeug>();

        public void AddWerkzeug(WerkzeugDto werkzeugDto)
        {
            var werkzeug = new Werkzeug
            {
                WerkzeugId = werkzeugDto.WerkzeugId,
                Marke = werkzeugDto.Marke,
                Art = werkzeugDto.Art,
                ProjektAdresse = werkzeugDto.ProjektAdresse,
                Beschreibung = werkzeugDto.Beschreibung
            };

            // Add the new tool to the list
            _werkzeugList.Add(werkzeug);
        }
        public List<Werkzeug> GetAllWerkzeuge()
        {
            return _werkzeugList;
        }

        public void UpdateAddressHistory(string werkzeugId, string newAddress)
        {
            var werkzeug = _werkzeugList.Find(w => w.WerkzeugId == werkzeugId);
            if (werkzeug != null)
            {
                // Add the new address to the history
                if (werkzeug.History.Count >= 5)
                {
                    werkzeug.History.RemoveAt(0); // Remove the oldest address
                }
                werkzeug.History.Add(newAddress);
            }
        }

        public List<string> GetAddressHistory(string werkzeugId)
        {
            var werkzeug = _werkzeugList.Find(w => w.WerkzeugId == werkzeugId);
            return werkzeug?.History ?? new List<string>();
        }
    }
}
