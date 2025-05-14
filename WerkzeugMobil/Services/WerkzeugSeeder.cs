using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;

namespace WerkzeugMobil.Services
{
    public class WerkzeugSeeder
    {
        private readonly WerkzeugDbContext _context;
        private readonly ILogger<WerkzeugSeeder> _logger;

        public WerkzeugSeeder(WerkzeugDbContext context, ILogger<WerkzeugSeeder> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void SeedWerkzeugeAndTools()
        {
            if (_context.Werkzeuge.Any() || _context.Tools.Any())
            {
                _logger.LogInformation("Werkzeuge or Tools already exist, skipping seeding.");
                return;
            }
            try
            {

                var werkzeuge = new List<WerkzeugDto>
            {
                new WerkzeugDto
                {
                    WerkzeugId = "WD-001",
                    Art = "Bohrmaschine",
                    Marke = "Bosch",
                    ProjektAdresse = "Gentzgasse",
                    Beschreibung = "Akku-Bohrmaschine 18V",
                    Lager = false,
                    History=null
                },
                new WerkzeugDto
                {
                    WerkzeugId = "SS-002",
                    Art = "Akkuschrauber",
                    Marke = "Makita",
                    ProjektAdresse = "Gentzgasse",
                    Beschreibung = "Kompakter Akkuschrauber",
                    Lager = false,
                    History=null
                },
                new WerkzeugDto
                {
                    WerkzeugId = "HP-003",
                    Art = "Schlagschrauber",
                    Marke = "DeWalt",
                    ProjektAdresse = "Gentzgasse",
                    Beschreibung = "Hochleistungs-Schlagschrauber",
                    Lager = false,
                    History=null
                },
                new WerkzeugDto
                {
                    WerkzeugId = "LD-004",
                    Art = "Trennschleifer",
                    Marke = "Hilti",
                    ProjektAdresse = "Gentzgasse",
                    Beschreibung = "Starker Trennschleifer für Beton",
                    Lager =false,
                    History=null
                },
                new WerkzeugDto
                {
                    WerkzeugId = "PP-005",
                    Art = "Laser",
                    Marke = "Bosch",
                    ProjektAdresse = "Gentzgasse",
                    Beschreibung = "Präzisionslaser mit Stativerfassung",
                    Lager =false,
                    History=null
                },
                new WerkzeugDto
                {
                    WerkzeugId = "OD-006",
                    Art = "Stemmhammer",
                    Marke = "Makita",
                    ProjektAdresse = "Gentzgasse",
                    Beschreibung = "Elektrischer Stemmhammer",
                    Lager =false,
                    History=null
                },
                new WerkzeugDto
                {
                    WerkzeugId = "WD-007",
                    Art = "Multitool",
                    Marke = "DeWalt",
                    ProjektAdresse = "Gentzgasse",
                    Beschreibung = "Allzweckwerkzeug für Innenausbau",
                    Lager =false,
                    History=null
                },
                new WerkzeugDto
                {
                    WerkzeugId = "HF",
                    Art = "Flex",
                    Marke = "",
                    ProjektAdresse = null,
                    Beschreibung = "OriginalZeile=Flex HF",
         Lager=false,
                    History=null
                },
new WerkzeugDto
{
    WerkzeugId = "HS",
    Art = "Säbelsäge",
    Marke = "",
     ProjektAdresse = null,
    Beschreibung = "OriginalZeile=Säbelsäge HS",
         Lager=false,
                    History=null

},
new WerkzeugDto
{
    WerkzeugId = "HS-1",
    Art = "Säbelsäge",
    Marke = "",
     ProjektAdresse = null,
    Beschreibung = "OriginalZeile=Säbelsäge HS-1",
         Lager=false,
                    History=null
},
new WerkzeugDto
{
    WerkzeugId = "HS-2",
    Art = "Säbelsäge",
    Marke = "",
     ProjektAdresse = null,
    Beschreibung = "OriginalZeile=Säbelsäge HS-2",
         Lager=false,
                    History=null
},
new WerkzeugDto
{
    WerkzeugId = "HS-3",
    Art = "Säbelsäge",
    Marke = "",
     ProjektAdresse = null,
    Beschreibung = "OriginalZeile=Säbelsäge HS-3",
         Lager=false,
                    History=null
},
new WerkzeugDto
{
    WerkzeugId = "H05",
    Art = "Abbruchhammer",
    Marke = "Hilti",
     ProjektAdresse = null,
    Beschreibung = "OriginalZeile=Abbru…",
         Lager=false,
                    History=null
},
new WerkzeugDto
    {
        WerkzeugId = "H05-01",
        Art = "Abbruchhammer",
        Marke = "Hilti 500er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=Gerät, Koffer=ja, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H05-02",
        Art = "Abbruchhammer",
        Marke = "Hilti 500er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=Gerät, Koffer=ja, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H05-03",
        Art = "Abbruchhammer",
        Marke = "Hilti 500er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=Gerät, Koffer=ja, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H05-04",
        Art = "Abbruchhammer",
        Marke = "Hilti 500er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H05-05",
        Art = "Abbruchhammer",
        Marke = "Hilti 500er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H05-06",
        Art = "Abbruchhammer",
        Marke = "Hilti 500er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H08-01",
        Art = "Abbruchhammer",
        Marke = "Hilti 800er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=Gerät, Koffer=ja, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H08-02",
        Art = "Abbruchhammer",
        Marke = "Hilti 800er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=Gerät, Koffer=ja, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H08-03",
        Art = "Abbruchhammer",
        Marke = "Hilti 800er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=Gerät, Koffer=ja, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H08-04",
        Art = "Abbruchhammer",
        Marke = "Hilti 800er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=Gerät, Koffer=ja, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H08-05",
        Art = "Abbruchhammer",
        Marke = "Hilti 800er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H10-01",
        Art = "Abbruchhammer",
        Marke = "Hilti 1000er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H10-02",
        Art = "Abbruchhammer",
        Marke = "Hilti 1000er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H10-03",
        Art = "Abbruchhammer",
        Marke = "Hilti 1000er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "H20-01",
        Art = "Abbruchhammer",
        Marke = "Hilti 2000er",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Bemerkung=",
         Lager=false,
                    History=null
    },

    new WerkzeugDto
    {
        WerkzeugId = "HF-1",
        Art = "Flex",
        Marke = "Hilti",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Ladegerät=, Batterie=, Sonstiges=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "HF-2",
        Art = "Flex",
        Marke = "Hilti",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Ladegerät=, Batterie=, Sonstiges=",
         Lager=false
    },
    new WerkzeugDto
    {
        WerkzeugId = "HF-3",
        Art = "Flex",
        Marke = "Hilti",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Ladegerät=, Batterie=, Sonstiges=",
         Lager=false
    },
    new WerkzeugDto
    {
        WerkzeugId = "EF-1",
        Art = "Flex",
        Marke = "Einhell",
         ProjektAdresse = null,
        Beschreibung = "Gerät=klein, Koffer=nein, Ladegerät=nein, Batterie=nein, Sonstiges=",
         Lager=false
    },
    new WerkzeugDto
    {
        WerkzeugId = "EF-2",
        Art = "Flex",
        Marke = "Einhell",
         ProjektAdresse = null,
        Beschreibung = "Gerät=klein, Koffer=nein, Ladegerät=nein, Batterie=nein, Sonstiges=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "EF-3",
        Art = "Flex",
        Marke = "Einhell",
         ProjektAdresse = null,
        Beschreibung = "Gerät=klein, Koffer=nein, Ladegerät=nein, Batterie=nein, Sonstiges=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "EF-4",
        Art = "Flex",
        Marke = "Einhell",
         ProjektAdresse = null,
        Beschreibung = "Gerät=Gross, Koffer=ja, Ladegerät=mit Kabel, Batterie=, Sonstiges=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "MF-1",
        Art = "Flex",
        Marke = "Makita",
         ProjektAdresse = null,
        Beschreibung = "Gerät=klein, Koffer=nein, Ladegerät=mit Kabel, Batterie=, Sonstiges=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "MF-2",
        Art = "Flex",
        Marke = "Makita",
         ProjektAdresse = null,
        Beschreibung = "Gerät=klein, Koffer=nein, Ladegerät=mit Kabel, Batterie=, Sonstiges=",
         Lager=false,
                    History=null
    },



    new WerkzeugDto
    {
        WerkzeugId = "HS-4",
        Art = "Säbelsäge",
        Marke = "Hilti",
         ProjektAdresse = null,
        Beschreibung = "Gerät=Gerät, Koffer=ja, Ladegerät=ja, Batterie=2X, Sonstiges=",
         Lager=false
    },
    new WerkzeugDto
    {
        WerkzeugId = "HS-5",
        Art = "Säbelsäge",
        Marke = "Hilti",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Ladegerät=, Batterie=, Sonstiges=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "HS-6",
        Art = "Säbelsäge",
        Marke = "Hilti",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Ladegerät=, Batterie=, Sonstiges=",
                    History=null
    },
     new WerkzeugDto
    {
        WerkzeugId = "HS-7",
        Art = "Säbelsäge",
        Marke = "Hilti",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Ladegerät=, Batterie=, Sonstiges=",
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "HS-8",
        Art = "Säbelsäge",
        Marke = "Hilti",
         ProjektAdresse = null,
        Beschreibung = "Gerät=, Koffer=, Ladegerät=, Batterie=, Sonstiges="
        ,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "STBS-1",
        Art = "Staubsauger",
        Marke = "Hilti",
        ProjektAdresse = null,
        Beschreibung = "Bemerkung=",
         Lager=false,
                    History=null
    },

    // Kompressor
    new WerkzeugDto
    {
        WerkzeugId = "KOMP-1",
        Art = "Kompressor",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Bemerkung=",
         Lager=false,
                    History=null
    },

    // Stromerzeuger
    new WerkzeugDto
    {
        WerkzeugId = "AGRE-1",
        Art = "Stromerzeuger",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Bemerkung=",
         Lager=false,
                    History=null
    },
    // Kabeltrommel
    new WerkzeugDto
    {
        WerkzeugId = "KT-1",
        Art = "Kabeltrommel",
        Marke = "Blau",
        ProjektAdresse = null,
        Beschreibung = "Länge=25m, mm2=1,5, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "KT-2",
        Art = "Kabeltrommel",
        Marke = "Blau",
        ProjektAdresse = null,
        Beschreibung = "Länge=25m, mm2=1,5, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "KT-3",
        Art = "Kabeltrommel",
        Marke = "Emos",
        ProjektAdresse = null,
        Beschreibung = "Länge=25m, mm2=1,5, Bemerkung=",
         Lager=false,
                    History=null
    },
   new WerkzeugDto
    {
        WerkzeugId = "KT-4",
        Art = "Kabeltrommel",
        Marke = "Emos",
        ProjektAdresse = null,
        Beschreibung = "Länge=25m, mm2=1,5, Bemerkung=",
         Lager=false,
                    History=null
    },

    // Kabel
   
    
  
   
   
    new WerkzeugDto
    {
        WerkzeugId = "K-5",
        Art = "Kabel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Länge=, mm2=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "K-6",
        Art = "Kabel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Länge=, mm2=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "K-7",
        Art = "Kabel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Länge=, mm2=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "K-8",
        Art = "Kabel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Länge=, mm2=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "K-9",
        Art = "Kabel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Länge=, mm2=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "K-10",
        Art = "Kabel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Länge=, mm2=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "K-11",
        Art = "Kabel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Länge=, mm2=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "K-12",
        Art = "Kabel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Länge=, mm2=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "K-13",
        Art = "Kabel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Länge=, mm2=, Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "K-14",
        Art = "Kabel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Länge=, mm2=, Bemerkung=",
         Lager=false,
                    History=null
    },
   new WerkzeugDto
    {
        WerkzeugId = "K-15",
        Art = "Kabel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Länge=, mm2=, Bemerkung=",
         Lager=false,
                    History=null
    },
   new WerkzeugDto
    {
        WerkzeugId = "K-16",
        Art = "Kabel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Länge=, mm2=, Bemerkung=",
         Lager=false,
                    History=null
    },
    // Werkzeugkasten
    new WerkzeugDto
    {
        WerkzeugId = "WK-1",
        Art = "Werkzeugkasten",
        Marke = "Vigor",
        ProjektAdresse = null,
        Beschreibung = "Bemerkung=Steckschlüsselset",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "WK-2",
        Art = "Werkzeugkasten",
        Marke = "Makita",
        ProjektAdresse = null,
        Beschreibung = "Bemerkung=Komplett Gross",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "WK-3",
        Art = "Werkzeugkasten",
        Marke = "Bosch",
        ProjektAdresse = null,
        Beschreibung = "Bemerkung=Bohrerset",
         Lager=true,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "WK-4",
        Art = "Werkzeugkasten",
        Marke = "Gedore",
        ProjektAdresse = null,
        Beschreibung = "Bemerkung=Steckschlüsselset",
         Lager=true,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "WK-5",
        Art = "Werkzeugkasten",
        Marke = "Gedore",
        ProjektAdresse = null,
        Beschreibung = "Bemerkung=Steckschlüsselset",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "WK-6",
        Art = "Werkzeugkasten",
        Marke = "ERBA",
        ProjektAdresse = null,
        Beschreibung = "Bemerkung=",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "S",
        Art = "Schaufel",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Schaufel",
         Lager=false,
                    History=null
    },
     new WerkzeugDto
    {
        WerkzeugId = "K-1",
        Art = "Krampen",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Krampen",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "K-2",
        Art = "Krampen",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Krampen",
         Lager=false
    },
    new WerkzeugDto
    {
        WerkzeugId = "K-3",
        Art = "Krampen",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Krampen",
         Lager=false,
                    History=null
    },
     new WerkzeugDto
    {
        WerkzeugId = "K-4",
        Art = "Krampen",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Krampen",
         Lager=false,
                    History=null
    },
     new WerkzeugDto
    {
        WerkzeugId = "HAM-1",
        Art = "Hammer",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Hammer",
         Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "B",
        Art = "Besen",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Besen",
         Lager=false,
                    History=null
    },
     new WerkzeugDto
    {
        WerkzeugId = "SE",
        Art = "Schaleisen",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Schaleisen",
         Lager=false,
                    History=null
    },
     new WerkzeugDto
    {
        WerkzeugId = "SB",
        Art = "Schaber",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Schaber",
         Lager=false,
                    History=null
    },
     new WerkzeugDto
    {
        WerkzeugId = "SR",
        Art = "Stoßscharre",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Stoßscharre",
         Lager=false,
                    History=null
    },
     new WerkzeugDto
    {
        WerkzeugId = "ST",
        Art = "Spitzspaten",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Spitzspaten",
         Lager=false,
                    History=null
    },
     new WerkzeugDto
    {
        WerkzeugId = "FS-1",
        Art = "Flachspaten",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Flachspaten",
        Lager=false,
                    History=null
    },
    new WerkzeugDto
    {
        WerkzeugId = "FS-2",
        Art = "Flachspaten",
        Marke = "",
        ProjektAdresse = null,
        Beschreibung = "Gerät=Flachspaten",
        Lager=false,
                    History=null
    }

};

                // Add each WerkzeugDto to the context
               


                var tools = new List<ToolDTO>
{
    new ToolDTO("HS", Tuple.Create("HS", 8)),
    new ToolDTO("H05", Tuple.Create("H05", 6)),
    new ToolDTO("H08", Tuple.Create("H08", 5)),
    new ToolDTO("H10", Tuple.Create("H10", 3)),
    new ToolDTO("H20", Tuple.Create("H20", 1)),
    new ToolDTO("HF", Tuple.Create("HF", 3)),
    new ToolDTO("EF", Tuple.Create("EF", 4)),
    new ToolDTO("MF", Tuple.Create("MF", 2)),
    new ToolDTO("STBS", Tuple.Create("STBS", 1)),
    new ToolDTO("KOMP", Tuple.Create("KOMP", 1)),
    new ToolDTO("AGRE", Tuple.Create("AGRE", 1)),
    new ToolDTO("KT", Tuple.Create("KT", 4)),
    new ToolDTO("K", Tuple.Create("K", 16)),
    new ToolDTO("WK", Tuple.Create("WK", 1))

        };

              


                _context.Werkzeuge.AddRange(werkzeuge);
                _context.Tools.AddRange(tools);
                _context.SaveChanges();

                _logger.LogInformation("Werkzeuge and Tools manually seeded successfully.");
            }


            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while saving Werkzeuge and Tools.");
            }
        }
    }
}