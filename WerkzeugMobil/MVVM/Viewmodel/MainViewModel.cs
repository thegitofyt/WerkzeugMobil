using ListDemo.ViewModels;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Windows.Input;
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
        private void LoadRandomWerkzeuge()
        {
            // Generate random Werkzeuge
            var random = new Random();
            var sampleWerkzeuge = Enumerable.Range(1, 20)
                                            .Select(i => new Werkzeug {WerkzeugId = $"Werkzeug {random.Next(1, 100)}" })
                                            .ToList();

            Werkzeuge = new ObservableCollection<Werkzeug>(sampleWerkzeuge.Take(5));
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
        private void ExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                // Show all Werkzeuge if search term is empty
                FilteredWerkzeuge = new ObservableCollection<Werkzeug>(Werkzeuge);
            }
            else
            {
                // Filter based on Marke, Art, or ProjektAdresse
                var filtered = Werkzeuge.Where(w =>
                    (w.Marke?.StartsWith(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (w.Art?.StartsWith(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (w.ProjektAdresse?.StartsWith(SearchTerm, StringComparison.OrdinalIgnoreCase) ?? false)
                ).ToList();

                FilteredWerkzeuge = new ObservableCollection<Werkzeug>(filtered);
            }
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
       
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
