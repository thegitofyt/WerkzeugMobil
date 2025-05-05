using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WerkzeugMobil.MVVM.Model;

namespace WerkzeugMobil.Services
{
    public class ToolsService
    {
        public List<Werkzeug> ListeWerkzeuge { get; set; } = new List<Werkzeug>();
        public Dictionary<string, Tools> ToolTypes { get; set; } = new Dictionary<string, Tools>();

        // Adds a new Werkzeug to the correct tool type
        //public void AddWerkzeug(string name, string art, string marke, string projektAdresse, string beschreibung)
        //{
        //    // Ensure that the tool name exists in the dictionary, otherwise add it
        //    if (!ToolTypes.ContainsKey(name))
        //    {
        //        ToolTypes[name] = new Tools();
        //    }

        //    // Get the next WerkzeugId based on the tool name
        //    string werkzeugId = ToolTypes[name].GetNextId();

        //    // Create a new Werkzeug and add it to the corresponding tool list
        //    var werkzeug = new Werkzeug();
        //    ToolTypes[name].ListeWerkzeuge.Add(werkzeug);
        //    ListeWerkzeuge.Add(werkzeug);  // Optionally, add it to the global list as well
        //}
    }
}
