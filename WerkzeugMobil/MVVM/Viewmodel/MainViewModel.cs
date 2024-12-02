using System.Collections.ObjectModel;
using WerkzeugMobil.MVVM.Model;
using WerkzeugMobil.MVVM.Viewmodel;
using WerkzeugMobil.Services;


namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class MainViewModel
    {
        public WerkzeugVerwaltung WerkzeugVerwaltung { get; set; }
        public Benutzer AngemeldeterBenutzer { get; set; }
        public ObservableCollection<Werkzeug> Werkzeuge { get; set; }

        public MainViewModel(WerkzeugMobilContext context)
        {
            WerkzeugVerwaltung = new WerkzeugVerwaltung(context);
            Werkzeuge = new ObservableCollection<Werkzeug>(context.Werkzeuge);
        }

        public void WerkzeugeHinzufügen(Werkzeug werkzeug)
        {
            if (AngemeldeterBenutzer.KannBearbeiten)
            {
                WerkzeugVerwaltung.Werkzeuge.Add(werkzeug);
                Werkzeuge.Add(werkzeug);
            }
        }

        public void WerkzeugeSuchen(string kriterium)
        {
            var gefundeneWerkzeuge = WerkzeugVerwaltung.SucheWerkzeuge(kriterium);
            Werkzeuge.Clear();
            foreach (var werkzeug in gefundeneWerkzeuge)
            {
                Werkzeuge.Add(werkzeug);
            }
        }
    }
}
