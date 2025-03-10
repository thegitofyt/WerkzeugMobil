using ListDemo.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;
using WerkzeugMobil.MVVM.Viewmodel;
using WerkzeugMobil.Services;


namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        //private readonly WerkzeugServices _werkzeugService;
        private ObservableCollection<Werkzeug>  werkzeugService;
        private Werkzeug _selectedWerkzeug;
        private Werkzeug _currentWerkzeug;



        private string _searchTerm; // Property to hold the search term
        private ObservableCollection<Werkzeug> _filteredWerkzeuge;

        public ObservableCollection<Werkzeug> Werkzeuge { get; set; }
        public ObservableCollection<string> History { get; set; }

        public Werkzeug currentWerkzeug
        {
            get => _currentWerkzeug;
            set
            {
                _currentWerkzeug = value;
                OnPropertyChanged(nameof(currentWerkzeug));
            }
        }
        //public Werkzeug SelectedWerkzeug
        //{
        //    get => _selectedWerkzeug;
        //    set
        //    {
        //        _selectedWerkzeug = value;
        //        OnPropertyChanged();
        //        UpdateAddressHistory();
        //    }
        //}

        public MainViewModel()
        {


            Werkzeuge = new ObservableCollection<Werkzeug>();
            FilteredWerkzeuge = new ObservableCollection<Werkzeug>(Werkzeuge);
            SearchCommand = new RelayCommand(ExecuteSearch);
            LoadRandomWerkzeuge();

            currentWerkzeug = new Werkzeug
            {
                WerkzeugId = "HS-1",
                Marke = "Bosch",
                Art = "Säbelsäge",
                ProjektAdresse = "123 Project St.",
                Beschreibung = "High-quality tool for heavy-duty tasks."
            };
           

            History = new ObservableCollection<string>
            {
                "Inspected on 2023-12-15",
                "Repaired on 2024-01-10",
                "Assigned to Project Alpha on 2024-02-20"
            };



            // Initialize the search command
            SearchCommand = new RelayCommand(ExecuteSearch);

            //_werkzeugService = new WerkzeugServices();

            // Initialize Werkzeug collection with sample data
           
    ;

            // Copy all Werkzeuge to the filtered list initially
            //FilteredWerkzeuge = new ObservableCollection<Werkzeug>(Werkzeuge);// Initialize with all Werkzeuge
            
            
        }

        //private void LoadRandomWerkzeuge()
        //{
        //    // Generate random Werkzeuge
        //    var random = new Random();
        //    var sampleWerkzeuge = Enumerable.Range(1, 20)
        //                                    .Select(i => new Werkzeug {WerkzeugId = $"Werkzeug {random.Next(1, 100)}" })
        //                                    .ToList();

        //    Werkzeuge = new ObservableCollection<Werkzeug>(sampleWerkzeuge.Take(5));
        //}


      
        public Werkzeug SelectedWerkzeug
        {
            get => _selectedWerkzeug;
            set
            {
                _selectedWerkzeug = value;
                currentWerkzeug = value; // Update the displayed Werkzeug
                OnPropertyChanged(nameof(SelectedWerkzeug));
            }
        }

        private void LoadRandomWerkzeuge()
        {
            using (var context = new WerkzeugDbContext())
            {
                // First, retrieve all the Werkzeuge from the database (client-side evaluation)
                var werkzeugeList = context.Werkzeuge
                                           .ToList() // Retrieve all items first
                                           .ToList(); // Make sure it's in a list for randomization

                // Create an instance of Random
                Random random = new Random();

                // Shuffle the list randomly
                werkzeugeList = werkzeugeList.OrderBy(x => random.Next()).Take(5).ToList();

                // Now assign the randomized items to ObservableCollection
                Werkzeuge = new ObservableCollection<Werkzeug>(werkzeugeList.Select(w => new Werkzeug
                {
                    WerkzeugId = w.WerkzeugId,
                    Marke = w.Marke,
                    Art = w.Art,
                    ProjektAdresse = w.ProjektAdresse,
                    Beschreibung = w.Beschreibung
                }));

                // Apply filtering if needed
                FilteredWerkzeuge = new ObservableCollection<Werkzeug>(Werkzeuge);
            }
        }

        public ObservableCollection<Werkzeug> FilteredWerkzeuge
        {
            get => _filteredWerkzeuge;
            set
            {
                _filteredWerkzeuge = value;
                OnPropertyChanged();
            }
        }
        //private void ExecuteSearch()
        //{
        //    if (string.IsNullOrWhiteSpace(SearchTerm))
        //    {
        //        // Show all Werkzeuge if search term is empty
        //        FilteredWerkzeuge = new ObservableCollection<Werkzeug>(Werkzeuge);
        //    }
        //    else
        //    {
        //        // Filter based on Marke, Art, or ProjektAdresse
        //        var filtered = Werkzeuge.Where(w =>
        //            (w.Marke?.StartsWith(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
        //            (w.Art?.StartsWith(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
        //            (w.ProjektAdresse?.StartsWith(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
        //        ).ToList();

        //        FilteredWerkzeuge = new ObservableCollection<Werkzeug>(filtered);
        //    }
        //}
        private void ExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                LoadRandomWerkzeuge(); // Reload all Werkzeuge
            }
            else
            {
                using (var context = new WerkzeugDbContext())
                {
                    var filteredWerkzeuge = context.Werkzeuge
                        .Where(w =>
                            EF.Functions.Like(w.Marke, $"%{SearchTerm}%") ||
                            EF.Functions.Like(w.Art, $"%{SearchTerm}%") ||
                            EF.Functions.Like(w.ProjektAdresse, $"%{SearchTerm}%") ||
                            EF.Functions.Like(w.WerkzeugId, $"%{SearchTerm}%") ||
                            EF.Functions.Like(w.Beschreibung, $"%{SearchTerm}%")
                        )
                        .ToList(); // Execute the query and get the result

                    // Assuming you have a mapping method for converting WerkzeugDto to Werkzeug
                    var mappedWerkzeuge = filteredWerkzeuge.Select(dto => new Werkzeug
                    {
                        // Mapping properties from WerkzeugDto to Werkzeug
                        Marke = dto.Marke,
                        Art = dto.Art,
                        ProjektAdresse = dto.ProjektAdresse,
                        WerkzeugId = dto.WerkzeugId,
                        Beschreibung = dto.Beschreibung
                    }).ToList();

                    // Update the ObservableCollection with the mapped result
                    Werkzeuge = new ObservableCollection<Werkzeug>(mappedWerkzeuge);
                    FilteredWerkzeuge = new ObservableCollection<Werkzeug>(Werkzeuge);
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged();
            }
        }
        public ICommand SearchCommand { get; private set; }


        //private void UpdateAddressHistory()
        ////{
        ////    History.Clear();
        ////    if (SelectedWerkzeug != null)
        ////    {
        ////        foreach (var address in SelectedWerkzeug.History)
        ////        {
        ////            History.Add(address);
        ////        }
        ////    }
        //}
       
       private IEnumerable<Werkzeug> GetFilteredWerkzeuge(string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return Werkzeuge;

            return Werkzeuge.Where(w =>
                (w.Marke?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (w.Art?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (w.ProjektAdresse?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (w.WerkzeugId?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                (w.Beschreibung?.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
            );
        }

        public event PropertyChangedEventHandler PropertyChanged;

        
    }
}
