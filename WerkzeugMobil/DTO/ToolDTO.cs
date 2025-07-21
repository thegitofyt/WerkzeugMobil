using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using WerkzeugMobil.Data;
using WerkzeugMobil.MVVM.Model;

public class ToolDTO
{
    [Key]
    public int Id { get; set; }

    public string Name { get; set; }  // Make this a property
   

    [NotMapped]
    public List<Tuple<string, int>> ToolTypeCounts { get; set; } = new();

    public string ToolTypeCountsSerialized
    {
        get => JsonSerializer.Serialize(ToolTypeCounts);
        set => ToolTypeCounts = JsonSerializer.Deserialize<List<Tuple<string, int>>>(value) ?? new();
    }

    public ToolDTO() { } // Parameterless constructor is required by EF

    // Optional constructor for use in your app (not for EF)
    public ToolDTO(string name, Tuple<string, int> toolType)
    {
        Name = name;
        ToolTypeCounts = new List<Tuple<string, int>> { toolType };
      
    }

   
}