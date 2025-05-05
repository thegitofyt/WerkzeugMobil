using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;
namespace WerkzeugMobil.MVVM.Model
{
    public class Tools
    {
       
        public int Id { get; set; }

        public String Name;

       
        public List<Tuple<string, int>> ToolTypeCounts { get; set; } = new List<Tuple<string, int>>();
        //public Dictionary<string, Werkzeug> KeyValuePairs { get; set; }
        // public Dictionary<string, int> LastUsedIds { get; set; } // Track last used ID for each Art
        // Create a mapped property to store serialized data
       
    }
}