using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;


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

                    // Seed Werkzeuge data if not already present
                    if (!context.Werkzeuge.Any())
                    {
                        var werkzeuge = new List<WerkzeugDto>
                        {
                            new WerkzeugDto { WerkzeugId = "HS-1", Marke = null, Art = "Säbelsäge", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "HS-2", Marke = null, Art = "Säbelsäge", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "HS-3", Marke = null, Art = "Säbelsäge", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H05-01", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H05-02", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H08-01", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H08-02", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H08-03", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H10", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "H20", Marke = "Hilti", Art = "Abbruchhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "K", Marke = null, Art = "Kettensäge", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "MIS-1", Marke = "Milwau", Art = "Schlagschrauber", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "MIB-1", Marke = "Milwau", Art = "Bohrhammer", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "ES", Marke = "Einhell", Art = "Akkuschrauber", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "KOM", Marke = null, Art = "Kompressor", ProjektAdresse = null, Beschreibung = null, Lager = false },
                            new WerkzeugDto { WerkzeugId = "STE", Marke = null, Art = "Stromerzeuger", ProjektAdresse = null, Beschreibung = null, Lager = false },
                        };
                        context.Werkzeuge.AddRange(werkzeuge);
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
    }
}