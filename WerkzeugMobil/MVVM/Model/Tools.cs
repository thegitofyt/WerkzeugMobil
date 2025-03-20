using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WerkzeugMobil.MVVM.Model
{
    public class Tools
    {
        public List <Werkzeug> ListeWerkzeuge { get; set; }
        // Dictionary to store tool categories and their corresponding lists of Werkzeuge
        public Dictionary <string, List<Werkzeug>> WerkzeugKategorien { get; set; }

        public Tools()
        {
            WerkzeugKategorien = new Dictionary<string, List<Werkzeug>>();
        }
        // Method to add a tool to the appropriate category
        public void AddTool(string werkzeugArt,Werkzeug werkzeug )
        {
            if(!WerkzeugKategorien.ContainsKey(werkzeugArt))
            {
                WerkzeugKategorien[werkzeugArt] = new List<Werkzeug>();
            }
            WerkzeugKategorien[werkzeugArt].Add(werkzeug);
        }
        // Method to get all tools of a specific type
        public List<Werkzeug> GetToolsByType(string werkzeugArt)
        {
            return WerkzeugKategorien.ContainsKey(werkzeugArt) ? WerkzeugKategorien[werkzeugArt] : new List<Werkzeug>();
        }
        // Method to get all tools
        public List<Werkzeug> GetAllTools()
        {
            return WerkzeugKategorien.Values.SelectMany(toolList => toolList).ToList();
        }


    }
}
