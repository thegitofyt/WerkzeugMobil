using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WerkzeugShared.DTO;
using WerkzeugShared.MVVM.Model;
using WerkzeugShared.Services;
using WerkzeugMobil.Converters;
using System.Windows;
using System.Collections.ObjectModel;
using WerkzeugMobil.Data;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using System.Globalization;

namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class AddWerkzeugViewModel : INotifyPropertyChanged
    {
        // This should be your binding target
        private ProjektDTO _selectedProjektAdresse;
        public ProjektDTO SelectedProjektAdresse
        {
            get => _selectedProjektAdresse;
            set
            {
                _selectedProjektAdresse = value;
                OnPropertyChanged();

                // Optional: update Werkzeug.ProjektAdresse if needed
                if (Werkzeug != null)
                {
                    Werkzeug.ProjektAdresse = value?.ProjektAddresse; // or null if "None"
                }
            }
        }

        private ObservableCollection<ProjektDTO> _projectAddresses;
        public ObservableCollection<ProjektDTO> ProjectAddresses
        {
            get => _projectAddresses;
            set
            {
                _projectAddresses = value;
                OnPropertyChanged();
            }
        }

        private readonly WerkzeugServices _werkzeugService;

        public event PropertyChangedEventHandler? PropertyChanged;

        private ObservableCollection<ToolDTO> _tools;

        public ICommand DeleteToolCommand { get; }


        public ICommand UpdateCommand { get; }

        public ICommand AddNewCommand { get; }
        public ICommand SubmitCommand { get; }
        private Tools _selectedTools;

        private Werkzeug _werkzeug;

        public Werkzeug Werkzeug
        {
            get => _werkzeug;
            set
            {
                _werkzeug = value;
                OnPropertyChanged();
            }
        }

        // Methode zum Befüllen der Felder mit den Werkzeug-Daten
        public void PopulateWerkzeug(Werkzeug werkzeug)
        {
            Werkzeug = werkzeug;
        }

        private ToolDTO _selectedTool;

        private async Task RefreshProjekteFromApiAsync()
        {
            try
            {
                var api = new WerkzeugApiService();
                var neueProjekte = await api.GetProjekteAsync();

                ProjectAddresses.Clear();
                foreach (var projekt in neueProjekte)
                {
                    ProjectAddresses.Add(projekt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Projekte von der API: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }





        public ToolDTO SelectedTool
        {
            get => _selectedTool;
            set
            {
                if (_selectedTool != value)
                {
                    _selectedTool = value;
                    OnPropertyChanged();

                    if (_selectedTool != null)
                    {
                        if (Werkzeug == null)
                            Werkzeug = new Werkzeug();

                        Werkzeug.WerkzeugId = TransformToolTypeCountsToString(_selectedTool.ToolTypeCounts);
                        Werkzeug.Art = _selectedTool.Name;

                        IsEditing = true; // ← hier!
                    }

                    OnPropertyChanged(nameof(Werkzeug));
                    ((RelayCommand)DeleteToolCommand).RaiseCanExecuteChanged();
                }
            }
        }
        private WerkzeugShared.WerkzeugDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WerkzeugShared.WerkzeugDbContext>();

            // Adjust path if needed - example using local app data folder
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = System.IO.Path.Combine(localAppData, "WerkzeugMobil", "WerkzeugMobilDb.sqlite");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new WerkzeugShared.WerkzeugDbContext(optionsBuilder.Options);
        }

        public AddWerkzeugViewModel()
        {
            // Dependency Injection
            var context = CreateDbContext();
            _werkzeugService = new WerkzeugServices(context);

            Werkzeug = new Werkzeug();
            AddNewCommand = new RelayCommand(AddNew);
            SubmitCommand = new RelayCommand(Submit);
            DeleteToolCommand = new RelayCommand(DeleteTool);
            LoadProjectAddresses();
            UpdateCommand = new RelayCommand(UpdateWerkzeug);

            LoadTools(); // Tools beim Initialisieren laden
        }


        private async void DeleteTool()
        {
            try
            {
                using (var ctx = CreateDbContext())
                {
                    // Wenn ein Tool ausgewählt ist
                    if (SelectedTool != null)
                    {
                        var entity = ctx.Tools.FirstOrDefault(t => t.Id == SelectedTool.Id);

                        if (entity == null)
                        {
                            MessageBox.Show("Tool nicht in der Datenbank gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }

                        // Werkzeug anhand WerkzeugId löschen
                        if (!string.IsNullOrWhiteSpace(Werkzeug?.WerkzeugId))
                        {
                            string id = Werkzeug.WerkzeugId.Trim();
                            var werkzeugEntity = ctx.Werkzeuge.FirstOrDefault(w => w.WerkzeugId == id);

                            if (werkzeugEntity != null)
                            {
                                ctx.Werkzeuge.Remove(werkzeugEntity);
                            }
                        }

                        ctx.Tools.Remove(entity);
                        int changes = ctx.SaveChanges();

                        if (changes > 0)
                        {
                            Tools.Remove(SelectedTool);
                            SelectedTool = null;
                            await RefreshToolsFromApiAsync();
                            MessageBox.Show("Tool und zugehöriges Werkzeug erfolgreich gelöscht!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Das Tool konnte nicht gelöscht werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    // Kein Tool ausgewählt, aber evtl. ein Werkzeug vorhanden
                    else if (!string.IsNullOrWhiteSpace(Werkzeug?.WerkzeugId))
                    {
                        string id = Werkzeug.WerkzeugId.Trim();
                        var werkzeugEntity = ctx.Werkzeuge.FirstOrDefault(w => w.WerkzeugId == id);

                        if (werkzeugEntity != null)
                        {
                            ctx.Werkzeuge.Remove(werkzeugEntity);
                            int changes = ctx.SaveChanges();

                            if (changes > 0)
                            {
                                MessageBox.Show("Werkzeug erfolgreich gelöscht!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Werkzeug konnte nicht gelöscht werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Werkzeug mit dieser ID nicht gefunden!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Bitte wählen Sie ein Tool oder geben Sie eine gültige Werkzeug-ID ein.", "Warnung", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Löschen: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void LoadProjectAddresses()
        {
            using (var ctx = CreateDbContext())
            {
                var projekts = ctx.Projekte
                    .Select(p => new ProjektDTO
                    {
                        ProjektAddresse = p.ProjektAddresse
                    }).ToList();

                // Add 'None' at the top (optional: make it null internally if needed)
                projekts.Insert(0, new ProjektDTO { ProjektAddresse = "None" });

                ProjectAddresses = new ObservableCollection<ProjektDTO>(projekts);
            }
            
            // Optionally set the selected item to "None"
            SelectedProjektAdresse = ProjectAddresses.FirstOrDefault();
            await RefreshProjekteFromApiAsync();
                }


        private async void UpdateAddressHistory(Werkzeug werkzeug)
        {
            if (string.IsNullOrWhiteSpace(werkzeug.ProjektAdresse))
                return;

            // Initialize History if it's null
            if (werkzeug.History == null)
                werkzeug.History = new List<string>();

            // Check if the address already contains a timestamp to prevent duplication
            bool alreadyHasTimestamp = werkzeug.ProjektAdresse.Contains("(") && werkzeug.ProjektAdresse.Contains(")");

            if (!alreadyHasTimestamp)
            {
                // Get current date and time
                var now = DateTime.Now.ToString("dd.MM.yyyy HH:mm");

                // Create new entry with timestamp
                var entry = $"{werkzeug.ProjektAdresse} ({now})";

                // Avoid duplicates (ignore timestamp in comparison)
                bool addressExists = werkzeug.History.Any(h => h.StartsWith(werkzeug.ProjektAdresse + " ("));
                if (!addressExists)
                {
                    // Insert new entry at the top of the list
                    werkzeug.History.Insert(0, entry);

                    // Keep only the latest 5 address entries (optional, adjust as needed)
                    if (werkzeug.History.Count > 5)
                        werkzeug.History = werkzeug.History.Take(5).ToList();
                }
            }

            // Save changes to the database (assuming you're using Entity Framework)
            using (var context = CreateDbContext())
            {
                context.Entry(werkzeug).State = EntityState.Modified;
                context.SaveChanges();
            }
            await FetchWerkzeugeAsync();
        }
        private async void UpdateWerkzeug()
        {
            try
            {
                // Check if the Werkzeug is available
                if (Werkzeug == null)
                {
                    MessageBox.Show("Kein Werkzeug zum Aktualisieren gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Update the address history first (before updating the Werkzeug)
                UpdateAddressHistory(Werkzeug);

                // Now update the Werkzeug itself
                using (var context = CreateDbContext())
                {
                    // Fetch the existing Werkzeug from the database
                    var existingWerkzeug = context.Werkzeuge
                        .FirstOrDefault(w => w.WerkzeugId == Werkzeug.WerkzeugId);

                    if (existingWerkzeug == null)
                    {
                        MessageBox.Show("Werkzeug nicht in der Datenbank gefunden!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Update the Werkzeug properties
                    existingWerkzeug.Marke = Werkzeug.Marke;
                    existingWerkzeug.Art = Werkzeug.Art;
                    existingWerkzeug.ProjektAdresse = Werkzeug.ProjektAdresse;
                    existingWerkzeug.Beschreibung = Werkzeug.Beschreibung;
                    existingWerkzeug.Lager = Werkzeug.Lager;
                    existingWerkzeug.History = Werkzeug.History; // Make sure the History is updated after the address is changed

                    // Mark the entity as modified
                    context.Werkzeuge.Update(existingWerkzeug);

                    // Save changes to the database
                    var result = context.SaveChanges();
                    await FetchWerkzeugeAsync();
                    if (result > 0)
                    {
                        MessageBox.Show("Werkzeug erfolgreich aktualisiert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadTools(); // Reload the list to reflect the changes
                    }
                    else
                    {
                        MessageBox.Show("Keine Änderungen gespeichert.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Aktualisieren: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }




        

        private async void AddNew()
        {
            Werkzeug = new Werkzeug();
            SelectedTool = null;
            IsEditing = false; // ← hier
            OnPropertyChanged(nameof(Werkzeug));
            await FetchWerkzeugeAsync();
        }

        private bool TryParseWerkzeugId(string input, out string toolType, out int quantity)
        {
            toolType = null;
            quantity = 0;

            if (string.IsNullOrWhiteSpace(input))
                return false;

            input = input.Trim();

            if (input.Contains("-"))
            {
                var parts = input.Split('-');
                if (parts.Length == 2 && !string.IsNullOrWhiteSpace(parts[0]) && int.TryParse(parts[1], out quantity))
                {
                    toolType = parts[0];
                    return true;
                }
            }
            else
            {
                // Match letters followed by digits, e.g., H1, Bohr200
                var match = Regex.Match(input, @"^([A-Za-z]+)(\d+)$");
                if (match.Success && int.TryParse(match.Groups[2].Value, out quantity))
                {
                    toolType = match.Groups[1].Value;
                    return true;
                }
            }

            return false;
        }

        private async void Submit()
        {
            try
            {
                if (Werkzeug == null)
                {
                    MessageBox.Show("Werkzeugdaten fehlen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Werkzeug.WerkzeugId))
                {
                    MessageBox.Show("Werkzeug-ID darf nicht leer sein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // WerkzeugId in standardisiertes Format bringen, falls noch nicht formatiert
                Werkzeug.WerkzeugId = EnsureWerkzeugIdFormatted(Werkzeug.WerkzeugId);

                if (!TryParseWerkzeugId(Werkzeug.WerkzeugId, out string toolType, out int quantity))
                {
                    MessageBox.Show("Werkzeug-ID muss wie 'Bohr200', 'Bohr-200', 'H1' oder 'H-1' formatiert sein.", "Formatfehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(Werkzeug.Art))
                {
                    MessageBox.Show("Bitte gib eine Werkzeug-Art ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Nur zur History hinzufügen, wenn vorhanden
                if (!string.IsNullOrWhiteSpace(Werkzeug.ProjektAdresse))
                {
                    if (Werkzeug.History == null)
                        Werkzeug.History = new List<string>();

                    if (!Werkzeug.History.Contains(Werkzeug.ProjektAdresse))
                        Werkzeug.History.Add(Werkzeug.ProjektAdresse);
                }
                if (Werkzeug.ProjektAdresse == "None")
                {
                    Werkzeug.Lager = true; // Set Lager to true
                }
                else
                {
                    Werkzeug.Lager = false; // Set Lager to false for other addresses
                }

                // DTO erstellen
                var werkzeugDto = new WerkzeugDto
                {
                    WerkzeugId = Werkzeug.WerkzeugId,
                    Art = Werkzeug.Art,
                    Marke = Werkzeug.Marke,
                    ProjektAdresse = Werkzeug.ProjektAdresse,
                    Beschreibung = Werkzeug.Beschreibung,
                    History = Werkzeug.History != null ? new List<string>(Werkzeug.History) : null,
                    
                };

                _werkzeugService.AddWerkzeug(werkzeugDto);
                await FetchWerkzeugeAsync();

                var newTool = new ToolDTO
                {
                    Name = Werkzeug.Art,
                    ToolTypeCounts = new List<Tuple<string, int>>
            {
                new Tuple<string, int>(toolType, quantity)
            }
                };

                using (var context = CreateDbContext())
                {
                    context.Tools.Add(newTool);
                    context.SaveChanges();
                    await RefreshToolsFromApiAsync();
                }

                Tools.Add(newTool);

                MessageBox.Show("Werkzeug erfolgreich hinzugefügt!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                AddNew(); // Formular zurücksetzen
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Hinzufügen des Werkzeugs: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private string EnsureWerkzeugIdFormatted(string input)
        {
            if (string.IsNullOrWhiteSpace(input)) return input;

            // Wenn schon ein Bindestrich vorhanden ist, nichts tun
            if (input.Contains("-")) return input;

            // Trenne Buchstaben und Zahlen automatisch
            int index = input.TakeWhile(char.IsLetter).Count();
            if (index == 0 || index == input.Length) return input; // Kein Format erkennbar

            string letters = input.Substring(0, index);
            string numbers = input.Substring(index);

            return $"{letters}-{numbers}";
        }

       



        private async void LoadTools()
        {
            try
            {
                using (var context = CreateDbContext())
                {
                    var toolsFromDb = context.Tools.ToList(); // Tools aus der DB holen

                    // Umwandeln der Tools aus der DB in ToolDTO und deserialisieren der ToolTypeCounts
                    _tools = new ObservableCollection<ToolDTO>(toolsFromDb.Select(t => new ToolDTO
                    {
                        Id = t.Id,
                        Name = t.Name,
                        ToolTypeCounts = JsonSerializer.Deserialize<List<Tuple<string, int>>>(t.ToolTypeCountsSerialized)
                    }));

                    Tools = _tools; // Setze die neue Liste in die ObservableCollection
                    OnPropertyChanged(nameof(Tools)); // Notify UI about the changes
                await  RefreshToolsFromApiAsync();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Werkzeuge: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task RefreshToolsFromApiAsync()
        {
            try
            {
                var api = new WerkzeugApiService();
                var neueTools = await api.GetToolsAsync();

                Tools.Clear();
                foreach (var tool in neueTools)
                {
                    Tools.Add(tool);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Tools von der API: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task<List<WerkzeugDto>> FetchWerkzeugeAsync()
        {
            try
            {
                var api = new WerkzeugApiService();
                return await api.GetWerkzeugeAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Abrufen der Werkzeuge: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return new List<WerkzeugDto>();
            }
        }


        private async void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = CreateDbContext())
                {
                    var werkzeugInDb = context.Werkzeuge.FirstOrDefault(w => w.WerkzeugId == Werkzeug.WerkzeugId);
                    if (werkzeugInDb != null)
                    {
                        // Werte aktualisieren
                        werkzeugInDb.Marke = Werkzeug.Marke;
                        werkzeugInDb.Art = Werkzeug.Art;
                        werkzeugInDb.ProjektAdresse = Werkzeug.ProjektAdresse;
                        werkzeugInDb.Beschreibung = Werkzeug.Beschreibung;

                        context.SaveChanges();
                        MessageBox.Show("Werkzeug erfolgreich aktualisiert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Werkzeug nicht gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    }
                }
                await FetchWerkzeugeAsync();
            }
             
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Aktualisieren des Werkzeugs: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


   


        private bool _isEditing;
        public bool IsEditing
        {
            get => _isEditing;
            set
            {
                _isEditing = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(SubmitButtonText)); // auch Text aktualisieren
            }
        }



        public string SubmitButtonText => IsEditing ? "Aktualisieren" : "Hinzufügen";


        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private string TransformToolTypeCountsToString(List<Tuple<string, int>> toolTypeCounts)
        {
            if (toolTypeCounts == null || !toolTypeCounts.Any())
                return string.Empty;

            return string.Join(", ", toolTypeCounts.Select(t => $"{t.Item1}-{t.Item2}"));
        }

        public ObservableCollection<ToolDTO> Tools
        {
            get { return _tools; }
            set
            {
                if (_tools != value)
                {
                    _tools = value;
                    OnPropertyChanged(nameof(Tools));
                }
            }
        }
    }
}
