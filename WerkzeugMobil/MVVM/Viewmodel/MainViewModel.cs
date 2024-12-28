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
        private readonly WerkzeugServices _werkzeugService;
        private Werkzeug _selectedWerkzeug;
        private string _searchTerm; // Property to hold the search term
        private ObservableCollection<Werkzeug> _filteredWerkzeuge;

        public ObservableCollection<Werkzeug> Werkzeuge { get; set; }
        public ObservableCollection<string> History { get; set; }

        public Werkzeug SelectedWerkzeug
        {
            get => _selectedWerkzeug;
            set
            {
                _selectedWerkzeug = value;
                OnPropertyChanged();
                UpdateAddressHistory();
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
        public MainViewModel()
        {
           
            _werkzeugService = new WerkzeugServices();
            Werkzeuge = new ObservableCollection<Werkzeug>(_werkzeugService.GetAllWerkzeuge());
#pragma warning disable IDE0028 // Initialisierung der Sammlung vereinfachen
            History = new ObservableCollection<string>();
#pragma warning restore IDE0028 // Initialisierung der Sammlung vereinfachen
            FilteredWerkzeuge = new ObservableCollection<Werkzeug>(Werkzeuge); // Initialize with all Werkzeuge
        }

        private void UpdateAddressHistory()
        {
            History.Clear();
            if (SelectedWerkzeug != null)
            {
                foreach (var address in SelectedWerkzeug.History)
                {
                    History.Add(address);
                }
            }
        }
        private void ExecuteSearch()
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                // If the search term is empty, show all Werkzeuge
                FilteredWerkzeuge = new ObservableCollection<Werkzeug>(Werkzeuge);
            }
            else
            {
                // Filter the Werkzeuge based on the search term
                var filtered = Werkzeuge
                    .Where(w => w.Marke.StartsWith(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                 w.Art.StartsWith(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                 w.ProjektAdresse.StartsWith(SearchTerm, StringComparison.OrdinalIgnoreCase))
                    .ToList();

                FilteredWerkzeuge = new ObservableCollection<Werkzeug>(filtered);
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
