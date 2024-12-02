using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Logging;



namespace WerkzeugMobil.MVVM.Model
{
    public class WerkzeugMobilContext
    {
        public List<Werkzeug> Werkzeuge { get; set; } = new List<Werkzeug>();
        public List<Projekt> Projekte { get; set; } = new List<Projekt>();
        public List<Benutzer> Benutzer { get; set; } = new List<Benutzer>();
    }
}

