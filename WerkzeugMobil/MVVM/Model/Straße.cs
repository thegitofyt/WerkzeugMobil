using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerkzeugMobil.MVVM.Model
{
    public class Straße
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual Werkzeug Werkzeug { get; set; }
        public virtual ICollection<Werkzeug> Werkzeuge { get; set; }

        public Straße(int id, string name, Werkzeug werkzeug, ICollection<Werkzeug> werkzeuge)
        {
            ID = id;
            Name = name;
            Werkzeug = werkzeug;
            Werkzeuge = werkzeuge;
        }
    }
}
