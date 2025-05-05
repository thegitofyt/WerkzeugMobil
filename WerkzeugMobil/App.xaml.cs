using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;


namespace WerkzeugMobil
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // Initialize the database
            try
            {
                using (var context = new WerkzeugDbContext())
                {

                    // Ensure migrations are applied
                    context.Database.Migrate();




                    if (!context.Projekte.Any())
                    {
                        var projects = new List<ProjektDTO>
                        {
                         new ProjektDTO
                        {
                        ProjektAddresse = "Strassergasse 34",
                        Werkzeuge = null // No tools added yet
                        },
                        new ProjektDTO
                        {
                        ProjektAddresse = "Gentzgasse",
                        Werkzeuge = null // No tools added yet
                        },
                        new ProjektDTO
                        {
                        ProjektAddresse = "Rosenhügelstrasse",
                        Werkzeuge = null // No tools added yet
                        },
                        new ProjektDTO
                        {
                        ProjektAddresse = "Obere Donau Strasse 63",
                        Werkzeuge = null // No tools added yet
                        }
                        };

                        context.Projekte.AddRange(projects);
                        context.SaveChanges();
                    }
                    // Seed Werkzeuge data if not already present
                    if (!context.Werkzeuge.Any())
                    {
                        var werkzeuge = new List<WerkzeugDto>
                        {
                            new WerkzeugDto { WerkzeugId = "HS-1", Marke = null, Art = "Säbelsäge", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "HS-2", Marke = null, Art = "Säbelsäge", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "HS-3", Marke = null, Art = "Säbelsäge", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "H05-01", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "H05-02", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "H08-01", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "H08-02", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "H08-03", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "H10", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "H20", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "K", Marke = null, Art = "Kettensäge", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "MIS-1", Marke = "Milwau", Art = "Schlagschrauber", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "MIB-1", Marke = "Milwau", Art = "Bohrhammer", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "ES", Marke = "Einhell", Art = "Akkuschrauber", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "KOM", Marke = null, Art = "Kompressor", ProjektAdresse = null, Beschreibung = null, Lager = true },
                            new WerkzeugDto { WerkzeugId = "STE", Marke = null, Art = "Stromerzeuger", ProjektAdresse = null, Beschreibung = null, Lager = true },
                        };
                        context.Werkzeuge.AddRange(werkzeuge);
                        context.SaveChanges();
                    }
                    if (!context.Tools.Any())
                    {
                        var tools = new List<ToolDTO>
                        {
                            new ToolDTO("Hammer", new Tuple<string, int>("HS", 1)),
                            new ToolDTO("Schaufel", new Tuple<string, int>("S", 1)),
                            new ToolDTO("Krampen", new Tuple<string, int>("K-1", 1)),
                            new ToolDTO("Krampen", new Tuple<string, int>("K-2", 1)),
                            new ToolDTO("Krampen", new Tuple<string, int>("K-3", 1)),
                            new ToolDTO("Krampen", new Tuple<string, int>("K-4", 1)),
                            new ToolDTO("Hammer", new Tuple<string, int>("HAM-1", 1)),
                            new ToolDTO("Besen", new Tuple<string, int>("B", 1)),
                            new ToolDTO("Schaleisen", new Tuple<string, int>("SE", 1)),
                            new ToolDTO("Schaber", new Tuple<string, int>("SB", 1)),
                            new ToolDTO("Stoßscharre", new Tuple<string, int>("SR", 1)),
                            new ToolDTO("Spitzspaten", new Tuple<string, int>("ST", 1)),
                            new ToolDTO("Flachspaten", new Tuple<string, int>("FS-1", 1)),
                            new ToolDTO("Flachspaten", new Tuple<string, int>("FS-2", 1))

                                };
                        context.Tools.AddRange(tools);
                        context.SaveChanges();
                    }

                    // Seed User data if not already present
                    if (!context.Benutzer.Any())
                    {
                        var users = new List<UserDTO>
                        {
                            CreateUser("admin1"),
                            CreateUser("admin2"),
                            CreateUser("admin3"),
                        };
                        context.Benutzer.AddRange(users);
                        context.SaveChanges();

                        var userDetails = new StringBuilder("User details:\n");
                        foreach (var user in users)
                        {
                            userDetails.AppendLine($"Username: {user.Benutzername}, Password: {user.Passwort}");
                        }
                        MessageBox.Show(userDetails.ToString(), "User Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                // Log detailed error messages, including all inner exceptions
                var errorMessage = new StringBuilder();
                errorMessage.AppendLine($"Exception: {ex.Message}");

                var inner = ex.InnerException;
                while (inner != null)
                {
                    errorMessage.AppendLine($"Inner Exception: {inner.Message}");
                    inner = inner.InnerException;
                }

                MessageBox.Show(errorMessage.ToString(), "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                // Optionally write the details to a file for further debugging
                File.WriteAllText("error_log.txt", errorMessage.ToString());

                Environment.Exit(1); // Exit the application on critical error
            }
        }

        private UserDTO CreateUser(string username)
        {
            return new UserDTO
            {
                Benutzername = username,
                Passwort = GeneratePassword(12) // Generate a 12-character password
            };
        }

        private string GeneratePassword(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            using (var rng = new RNGCryptoServiceProvider())
            {
                var data = new byte[length];
                rng.GetBytes(data);
                var result = new StringBuilder(length);
                foreach (var b in data)
                {
                    result.Append(chars[b % chars.Length]);
                }
                return result.ToString();
            }
        }

        public List<Werkzeug> GetAllWerkzeuge()
        {
            using (var context = new WerkzeugDbContext())
            {
                // Fetch all WerkzeugDto from the database and map them to Werkzeug
                var werkzeugeDtos = context.Werkzeuge.ToList(); // List of WerkzeugDto

                // Map the WerkzeugDto to Werkzeug
                var werkzeuge = werkzeugeDtos.Select(dto => new Werkzeug
                {
                    WerkzeugId = dto.WerkzeugId,
                    Marke = dto.Marke,
                    Art = dto.Art,
                    ProjektAdresse = dto.ProjektAdresse,
                    Beschreibung = dto.Beschreibung
                }).ToList();

                return werkzeuge; // Return the List<Werkzeug>
            }
        }
    }
}