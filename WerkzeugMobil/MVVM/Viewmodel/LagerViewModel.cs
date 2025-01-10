using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WerkzeugMobil.DTO;

namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class LagerViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<WerkzeugDto> Werkzeuge { get; set; }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public LagerViewModel()
        {
            // Initialize Werkzeuge (e.g., load from database)
            Werkzeuge = new ObservableCollection<WerkzeugDto>
            {
                new WerkzeugDto { WerkzeugId = "HS-1", Marke = null, Art = "Säbelsäge", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "HS-2", Marke = null, Art = "Säbelsäge", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "HS-3", Marke = null, Art = "Säbelsäge", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H05-01", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H05-02", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H08-01", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H08-02", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H08-03", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H10", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H20", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "K", Marke = null, Art = "Kettensäge", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "MIS-1", Marke = "Milwau", Art = "Schlagschrauber", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "MIB-1", Marke = "Milwau", Art = "Bohrhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "ES", Marke = "Einhell", Art = "Akkuschrauber", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "KOM", Marke = null, Art = "Kompressor", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "STE", Marke = null, Art = "Stromerzeuger", ProjektAdresse = null, Beschreibung = null, Lager = false },
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
