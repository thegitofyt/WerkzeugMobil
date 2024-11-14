using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerkzeugMobil.MVVM.Model
{
    public class Werkzeug
    {
        public enum Gerät
        {
            Groß,
            Klein,
            Gerät
        }
        public int ID { get; set; }
        public string Name { get; set; }
        public string Marke { get; set; }
        public string Art { get; set; }
        public string Bemerkung { get; set; }
        public Gerät GerätGröße { get; set; }
        public bool? Koffer { get; set; }
        public bool? Ladegerät { get; set; }
        public int? Batterie { get; set; }
        public int? Länge { get; set; }
        public double? Mm2 { get; set; }

        public Straße Straße { get; set; }

        public Werkzeug(int id, string name, string marke, string art, string bemerkung, Straße straße
                        , Gerät gerät, bool? koffer, bool? ladegerät, int? batterie, int? länge, double? mm2)
        {
            ID = id;
            Name = name;
            Marke = marke;
            Art = art;
            Bemerkung = bemerkung;
            Straße = straße;
            GerätGröße = gerät;
            Koffer = koffer;
            Ladegerät = ladegerät;
            Batterie = batterie;
            Länge = länge;
            Mm2 = mm2;
        }
    }
}
