using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;
using WerkzeugMobil.Services;
using ListDemo.ViewModels;
using System.Windows;
using System.Collections.ObjectModel;
using WerkzeugMobil.Data;
using System.Text.Json;
using System.Diagnostics;

namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class AddWerkzeugViewModel : INotifyPropertyChanged
    {
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

        private object _selectedToolss;
        public object SelectedToolss
        {
            get => _selectedToolss;
            set
            {
                if (_selectedToolss != value)
                {
                    _selectedToolss = value;
                    OnPropertyChanged(nameof(SelectedToolss));
                }
            }
        }


        //public ToolDTO SelectedTool
        //{
        //    get => _selectedTool;
        //    set
        //    {
        //        if (_selectedTool != value)
        //        {
        //            _selectedTool = value;
        //            OnPropertyChanged(nameof(SelectedTool));

        //            if (_selectedTool != null)
        //            {
        //                if (Werkzeug == null)
        //                    Werkzeug = new Werkzeug();

        //                Werkzeug.WerkzeugId = _selectedTool.Id.ToString(); // <-- wichtig!
        //                Werkzeug.Art = _selectedTool.Name;

        //                // NICHT resetten: Marke, ProjektAdresse etc.

        //                OnPropertyChanged(nameof(Werkzeug));
        //            }


        //            OnPropertyChanged(nameof(SelectedTool)); // Notify UI about the change
        //        }
        //    }
        //}

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

                        Werkzeug.WerkzeugId = _selectedTool.Id.ToString();
                        Werkzeug.Art = _selectedTool.Name;

                        IsEditing = true; // ← hier!
                    }

                    OnPropertyChanged(nameof(Werkzeug));
                }
            }
        }


        public AddWerkzeugViewModel()
        {
            // Dependency Injection
            _werkzeugService = new WerkzeugServices();

            Werkzeug = new Werkzeug();
            AddNewCommand = new RelayCommand(AddNew);
            SubmitCommand = new RelayCommand(Submit);
            DeleteToolCommand = new RelayCommand(DeleteTool);
            UpdateCommand = new RelayCommand(UpdateWerkzeug);

            LoadTools(); // Tools beim Initialisieren laden
        }

        private void DeleteTool(object parameter)
        {
            if (!(parameter is ToolDTO tool))
                return;

            try
            {
                using (var ctx = new WerkzeugDbContext())
                {
                    // Suche das Tool in der DB
                    var entity = ctx.Tools.FirstOrDefault(t => t.Id == tool.Id);
                    if (entity == null)
                    {
                        MessageBox.Show("Tool nicht in der Datenbank gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    // Lösche es und speichere
                    ctx.Tools.Remove(entity);
                    int changes = ctx.SaveChanges();

                    if (changes > 0)
                    {
                        // UI-Liste aktualisieren
                        Tools.Remove(tool);
                        MessageBox.Show("Tool erfolgreich gelöscht!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Das Tool konnte nicht gelöscht werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Löschen: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        private void UpdateWerkzeug()
        {
            try
            {
                // Remove the incorrect usage of DataContext  
                // Directly use the current instance of AddWerkzeugViewModel  

                if (Werkzeug == null)
                {
                    MessageBox.Show("Kein Werkzeug zum Aktualisieren gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                using (var context = new WerkzeugDbContext())
                {
                    // Fetch the Werkzeug from the database  
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
                    existingWerkzeug.History = Werkzeug.History;



                    context.Werkzeuge.Update(existingWerkzeug); // Update the existing Werkzeug entity


                    // Save changes to the database  
                    var result = context.SaveChanges();

                    if (result > 0)
                    {
                        MessageBox.Show("Werkzeug erfolgreich aktualisiert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadTools(); // Reload the list  
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




        // Laden der Werkzeuge aus der Datenbank
        //private void LoadTools()
        //{
        //    try
        //    {
        //        using (var context = new WerkzeugDbContext())
        //        {
        //            var toolsFromDb = context.Tools.ToList(); // Tools aus der DB holen

        //            // Umwandeln der Tools aus der DB in ToolDTO und deserialisieren der ToolTypeCounts
        //            _tools = new ObservableCollection<ToolDTO>(toolsFromDb.Select(t => new ToolDTO
        //            {
        //                Id = t.Id,
        //                Name = t.Name,
        //                ToolTypeCounts = JsonSerializer.Deserialize<List<Tuple<string, int>>>(t.ToolTypeCountsSerialized)
        //            }));

        //            Tools = new ObservableCollection<ToolDTO>(_tools); // Setze Tools in die ObservableCollection

        //            OnPropertyChanged(nameof(Tools)); // Notify UI about the changes
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Fehler beim Laden der Werkzeuge: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}


        //private void LoadTools()
        //{
        //    try
        //    {
        //        using (var context = new WerkzeugDbContext())
        //        {
        //            var toolsFromDb = context.Tools.ToList();

        //            _tools = new ObservableCollection<ToolDTO>(toolsFromDb.Select(t => new ToolDTO
        //            {
        //                Id = t.Id,
        //                Name = t.Name,
        //                ToolTypeCounts = JsonSerializer.Deserialize<List<Tuple<string, int>>>(t.ToolTypeCountsSerialized)
        //            }));

        //            Tools = new ObservableCollection<ToolDTO>(_tools);
        //            OnPropertyChanged(nameof(Tools));
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Fehler beim Laden der Werkzeuge: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        //private void LoadTools()
        //{
        //    try
        //    {
        //        using (var context = new WerkzeugDbContext())
        //        {
        //            var toolsFromDb = context.Tools.ToList();

        //            // Debugging: Ausgabe der geladenen Tools
        //            foreach (var tool in toolsFromDb)
        //            {
        //                Debug.WriteLine($"Tool loaded: {tool.Name}");
        //            }

        //            _tools = new ObservableCollection<ToolDTO>(toolsFromDb.Select(t => new ToolDTO
        //            {
        //                Id = t.Id,
        //                Name = t.Name,
        //                ToolTypeCounts = JsonSerializer.Deserialize<List<Tuple<string, int>>>(t.ToolTypeCountsSerialized)
        //            }));

        //            Tools = _tools;
        //            OnPropertyChanged(nameof(Tools)); // UI benachrichtigen
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Fehler beim Laden der Werkzeuge: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}



        //private void AddNew()
        //{
        //    Werkzeug = new Werkzeug(); // Reset the form
        //    OnPropertyChanged(nameof(Werkzeug));
        //}
        //public string GeraeteKennzahl
        //{
        //    get => SelectedTool?.ToolTypeCounts?.FirstOrDefault()?.Item1 ?? string.Empty;
        //}
        //private void AddNew()
        //{
        //    Werkzeug = new Werkzeug();
        //    SelectedTool = null;
        //    IsEditing = false; // ← hier
        //    OnPropertyChanged(nameof(Werkzeug));
        //}

        private void AddNew()
        {
            Werkzeug = new Werkzeug();
            SelectedTool = null;
            IsEditing = false; // ← hier
            OnPropertyChanged(nameof(Werkzeug));
        }



        private void Submit()
        {
            try
            {
                var werkzeugDto = new WerkzeugDto
                {
                    WerkzeugId = Werkzeug.WerkzeugId,
                    Marke = Werkzeug.Marke,
                    Art = Werkzeug.Art,
                    ProjektAdresse = Werkzeug.ProjektAdresse,
                    Beschreibung = Werkzeug.Beschreibung
                };

                // Werkzeug in der Datenbank speichern
                _werkzeugService.AddWerkzeug(werkzeugDto);

                // Das Werkzeug direkt zur Liste hinzufügen, um die UI sofort zu aktualisieren
                var newTool = new ToolDTO
                {
                    Name = Werkzeug.Art,
                    ToolTypeCounts = new List<Tuple<string, int>>
                    {
                        new Tuple<string, int>(Werkzeug.Art, 1)
                    }
                };

                using (var context = new WerkzeugDbContext())
                {
                    context.Tools.Add(newTool);
                    context.SaveChanges();
                }

                Tools.Add(newTool); // UI-Liste aktualisieren
                MessageBox.Show("Werkzeug erfolgreich hinzugefügt!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

                // Formular zurücksetzen
                AddNew();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Hinzufügen des Werkzeugs: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //private void Submit()
        //{
        //    try
        //    {
        //        var werkzeugDto = new WerkzeugDto
        //        {
        //            WerkzeugId = Werkzeug.WerkzeugId,
        //            Marke = Werkzeug.Marke,
        //            Art = Werkzeug.Art,
        //            ProjektAdresse = Werkzeug.ProjektAdresse,
        //            Beschreibung = Werkzeug.Beschreibung
        //        };

        //        using (var context = new WerkzeugDbContext())
        //        {
        //            // Wandeln der int WerkzeugId zu string, um mit der string Id von existingTool zu vergleichen
        //            var existingTool = context.Tools.FirstOrDefault(t => t.Id.ToString() == Werkzeug.WerkzeugId);

        //            if (existingTool != null)
        //            {
        //                // ID existiert – aktualisiere die Felder
        //                existingTool.Name = Werkzeug.Art;
        //                existingTool.ToolTypeCounts = new List<Tuple<string, int>>
        //        {
        //            new Tuple<string, int>(Werkzeug.Art, 1)
        //        };

        //                context.SaveChanges();
        //                MessageBox.Show("Werkzeug wurde aktualisiert!", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            else
        //            {
        //                // Fix for CS0029: Convert Werkzeug.WerkzeugId to int before assigning to ToolDTO.Id  
        //                var newTool = new ToolDTO
        //                {
        //                    Id = int.Parse(Werkzeug.WerkzeugId), // Explicitly convert string to int  
        //                    Name = Werkzeug.Art,
        //                    ToolTypeCounts = new List<Tuple<string, int>>
        //                   {
        //                       new Tuple<string, int>(Werkzeug.Art, 1)
        //                   }
        //                };

        //                context.Tools.Add(newTool);
        //                context.SaveChanges();
        //                Tools.Add(newTool); // UI-Liste aktualisieren
        //                MessageBox.Show("Werkzeug erfolgreich hinzugefügt!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //        }

        //        // Werkzeug auch im Service aktualisieren oder neu hinzufügen
        //        _werkzeugService.AddWerkzeug(werkzeugDto);

        //        // Formular zurücksetzen
        //        AddNew();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Fehler beim Hinzufügen des Werkzeugs: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}


        //private void Submit()
        //{
        //    try
        //    {
        //        var werkzeugDto = new WerkzeugDto
        //        {
        //            WerkzeugId = Werkzeug.WerkzeugId,
        //            Marke = Werkzeug.Marke,
        //            Art = Werkzeug.Art,
        //            ProjektAdresse = Werkzeug.ProjektAdresse,
        //            Beschreibung = Werkzeug.Beschreibung
        //        };

        //        using (var context = new WerkzeugDbContext())
        //        {
        //            // Überprüfe, ob ein Werkzeug mit derselben WerkzeugId bereits existiert
        //            var existingTool = context.Tools.FirstOrDefault(t => t.Id.ToString() == Werkzeug.WerkzeugId);

        //            if (existingTool != null)
        //            {
        //                // Wenn die WerkzeugId bereits existiert, aktualisiere die anderen Felder
        //                existingTool.Name = Werkzeug.Art;

        //                // Speichern der Änderungen
        //                context.SaveChanges();
        //                MessageBox.Show("Werkzeug wurde aktualisiert!", "Update", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            else
        //            {
        //                // Fix for CS0029: Convert Werkzeug.WerkzeugId (string) to int before assigning to ToolDTO.Id  
        //                var newTool = new ToolDTO
        //                {
        //                    Id = int.Parse(Werkzeug.WerkzeugId), // Explicitly convert string to int  
        //                    Name = Werkzeug.Art
        //                };

        //                context.Tools.Add(newTool);
        //                context.SaveChanges();
        //                Tools.Add(newTool); // UI-Liste aktualisieren
        //                MessageBox.Show("Werkzeug erfolgreich hinzugefügt!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //        }

        //        // Werkzeug auch im Service aktualisieren oder neu hinzufügen
        //        _werkzeugService.AddWerkzeug(werkzeugDto);

        //        // Formular zurücksetzen
        //        AddNew();
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Fehler beim Hinzufügen des Werkzeugs: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}



        //private void Submit()
        //{
        //    try
        //    {
        //        var werkzeugDto = new WerkzeugDto
        //        {
        //            WerkzeugId = Werkzeug.WerkzeugId,
        //            Marke = Werkzeug.Marke,
        //            Art = Werkzeug.Art,
        //            ProjektAdresse = Werkzeug.ProjektAdresse,
        //            Beschreibung = Werkzeug.Beschreibung
        //        };

        //        using (var context = new WerkzeugDbContext())
        //        {
        //            using (var transaction = context.Database.BeginTransaction())
        //            {
        //                // Prüfen, ob das Werkzeug mit der ID schon existiert  
        //                var existingTool = context.Tools.FirstOrDefault(t => t.Id == int.Parse(werkzeugDto.WerkzeugId));

        //                if (existingTool != null)
        //                {
        //                    // Update der Felder  
        //                    existingTool.Name = werkzeugDto.Art;
        //                    // Hier weitere Felder ergänzen, falls nötig  
        //                    context.SaveChanges();
        //                }
        //                else
        //                {
        //                    // Neues Werkzeug hinzufügen  
        //                    var newTool = new ToolDTO
        //                    {
        //                        Id = int.Parse(werkzeugDto.WerkzeugId), // Explizite Konvertierung von string zu int  
        //                        Name = werkzeugDto.Art,
        //                        ToolTypeCounts = new List<Tuple<string, int>>
        //                       {
        //                           new Tuple<string, int>(werkzeugDto.Art, 1)
        //                       }
        //                    };
        //                    context.Tools.Add(newTool);
        //                    context.SaveChanges();

        //                    Tools.Add(newTool); // UI-Liste aktualisieren  
        //                }

        //                transaction.Commit(); // Änderungen dauerhaft speichern  
        //            }
        //        }

        //        MessageBox.Show("Werkzeug erfolgreich hinzugefügt oder aktualisiert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

        //        AddNew(); // Formular zurücksetzen  
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Fehler beim Hinzufügen/Aktualisieren des Werkzeugs: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}



        //private void LoadTools()
        //{
        //    try
        //    {
        //        using (var context = new WerkzeugDbContext())
        //        {
        //            var toolsFromDb = context.Tools.ToList(); // Tools aus der DB holen

        //            // Umwandeln der Tools aus der DB in ToolDTO
        //            _tools = new ObservableCollection<ToolDTO>(toolsFromDb.Select(t => new ToolDTO
        //            {
        //                Id = t.Id,
        //                Name = t.Name,
        //                ToolTypeCounts = JsonSerializer.Deserialize<List<Tuple<string, int>>>(t.ToolTypeCountsSerialized)
        //            }));

        //            Tools = _tools; // Setze die Tools in die ObservableCollection

        //            OnPropertyChanged(nameof(Tools)); // Notify UI about the changes
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Fehler beim Laden der Werkzeuge: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}



        private void LoadTools()
        {
            try
            {
                using (var context = new WerkzeugDbContext())
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
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Werkzeuge: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var context = new WerkzeugDbContext())
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
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Aktualisieren des Werkzeugs: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        //private void Submit()
        //{
        //    try
        //    {
        //        // Werkzeug hinzufügen oder aktualisieren (je nach Bedarf)
        //        using (var context = new WerkzeugDbContext())
        //        {
        //            var existingWerkzeug = context.Werkzeuge.FirstOrDefault(w => w.WerkzeugId == Werkzeug.WerkzeugId);

        //            if (existingWerkzeug != null)
        //            {
        //                // Aktualisierung

        //                existingWerkzeug.Marke = Werkzeug.Marke;
        //                existingWerkzeug.Art = Werkzeug.Art;
        //                existingWerkzeug.ProjektAdresse = Werkzeug.ProjektAdresse;
        //                existingWerkzeug.Beschreibung = Werkzeug.Beschreibung;
        //                context.SaveChanges();
        //                MessageBox.Show("Werkzeug wurde aktualisiert!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            else
        //            {
        //                // Hinzufügen eines neuen Werkzeugs
        //                var neuesWerkzeug = new WerkzeugDto
        //                {
        //                    WerkzeugId = Werkzeug.WerkzeugId,
        //                    Marke = Werkzeug.Marke,
        //                    Art = Werkzeug.Art,
        //                    ProjektAdresse = Werkzeug.ProjektAdresse,
        //                    Beschreibung = Werkzeug.Beschreibung
        //                };
        //                _werkzeugService.AddWerkzeug(neuesWerkzeug);
        //                MessageBox.Show("Werkzeug wurde hinzugefügt!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //        }

        //        // Tools nach dem Hinzufügen oder Aktualisieren neu laden
        //        LoadTools();
        //        AddNew();  // Formular zurücksetzen

        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Fehler beim Speichern des Werkzeugs: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}



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
