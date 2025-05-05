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
        private ObservableCollection<Werkzeug> werkzeugService;
        private Werkzeug _selectedWerkzeug;
        private Werkzeug _currentWerkzeug;
        private ProjektDTO _selectedProjekt;
        public ProjekteViewModel ProjekteViewModel { get; set; }

        private ProjekteViewModel _projekteViewModel;
        private AddProjektViewModel _addProjektViewModel;

        private string _searchTerm; // Property to hold the search term
        private ObservableCollection<Werkzeug> _filteredWerkzeuge;

        public ObservableCollection<Werkzeug> Werkzeuge { get; set; }
        public ObservableCollection<string> History { get; set; }

        public ProjekteViewModel ProjekteViewModels
        {
            get { return _projekteViewModel; }
            set
            {
                _projekteViewModel = value;
                OnPropertyChanged(nameof(ProjekteViewModels));
            }
        }
        public AddProjektViewModel AddProjektViewModel
        {
            get { return _addProjektViewModel; }
            set
            {
                _addProjektViewModel = value;
                OnPropertyChanged(nameof(AddProjektViewModel));
            }
        }
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
            _projekteViewModel = new ProjekteViewModel();
            _addProjektViewModel = new AddProjektViewModel();

            ProjekteViewModel = new ProjekteViewModel();
            ProjekteViewModel.SetMainViewModel(this);
            Werkzeuge = new ObservableCollection<Werkzeug>();
            FilteredWerkzeuge = new ObservableCollection<Werkzeug>(Werkzeuge);
            SearchCommand = new RelayCommand(ExecuteSearch);
            SelectProjektCommand = new RelayCommand(SelectProjekt);
            LoadWerkzeugeForProject(SelectedProjekt);
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


        public ProjektDTO SelectedProjekt
        {
            get => _selectedProjekt;
            set
            {
                _selectedProjekt = value;
                OnPropertyChanged(nameof(SelectedProjekt));
                LoadWerkzeugeForProject(SelectedProjekt);
            }
        }
        private void SelectProjekt()
        {
            if (SelectedProjekt != null)
            {
                LoadWerkzeugeForProject(SelectedProjekt);
            }
        }
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
            if (SelectedProjekt == null || string.IsNullOrWhiteSpace(SelectedProjekt.ProjektAddresse))
            {
                return;
            }

            using (var context = new WerkzeugDbContext())
            {
                var werkzeugeList = context.Werkzeuge
                                           .Where(w => w.ProjektAdresse == SelectedProjekt.ProjektAddresse)
                                           .ToList();

                if (werkzeugeList.Any())
                {
                    Random random = new Random();
                    werkzeugeList = werkzeugeList.OrderBy(x => random.Next()).Take(5).ToList();

                    var mappedWerkzeuge = werkzeugeList.Select(w => new Werkzeug
                    {
                        WerkzeugId = w.WerkzeugId,
                        Marke = w.Marke,
                        Art = w.Art,
                        ProjektAdresse = w.ProjektAdresse,
                        Beschreibung = w.Beschreibung
                    }).ToList();

                    Werkzeuge = new ObservableCollection<Werkzeug>(mappedWerkzeuge);
                    FilteredWerkzeuge = new ObservableCollection<Werkzeug>(Werkzeuge);

                    OnPropertyChanged(nameof(Werkzeuge));
                    OnPropertyChanged(nameof(FilteredWerkzeuge));
                }
                else
                {
                    Werkzeuge.Clear();
                    FilteredWerkzeuge.Clear();

                    OnPropertyChanged(nameof(Werkzeuge));
                    OnPropertyChanged(nameof(FilteredWerkzeuge));
                }
            }
        }


        public void LoadWerkzeugeForProject(ProjektDTO selectedProjekt)
        {
            if (selectedProjekt == null) return;

            using (var context = new WerkzeugDbContext())
            {
                var werkzeugeList = context.Werkzeuge
                    .Where(w => w.ProjektAdresse == selectedProjekt.ProjektAddresse)
                    .ToList();

                var mappedWerkzeuge = werkzeugeList.Select(w => new Werkzeug
                {
                    WerkzeugId = w.WerkzeugId,
                    Marke = w.Marke,
                    Art = w.Art,
                    ProjektAdresse = w.ProjektAdresse,
                    Beschreibung = w.Beschreibung
                }).ToList();

                Werkzeuge = new ObservableCollection<Werkzeug>(mappedWerkzeuge);
                FilteredWerkzeuge = new ObservableCollection<Werkzeug>(Werkzeuge);
                OnPropertyChanged(nameof(Werkzeuge));
                OnPropertyChanged(nameof(FilteredWerkzeuge));
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
            if (SelectedProjekt == null || string.IsNullOrWhiteSpace(SelectedProjekt.ProjektAddresse))
            {
                FilteredWerkzeuge = new ObservableCollection<Werkzeug>();
                return;
            }

            using (var context = new WerkzeugDbContext())
            {
                var filteredWerkzeuge = context.Werkzeuge
                    .Where(w => w.ProjektAdresse == SelectedProjekt.ProjektAddresse &&
                        (EF.Functions.Like(w.Marke, $"%{SearchTerm}%") ||
                         EF.Functions.Like(w.Art, $"%{SearchTerm}%") ||
                         EF.Functions.Like(w.WerkzeugId.ToString(), $"%{SearchTerm}%") ||
                         EF.Functions.Like(w.Beschreibung, $"%{SearchTerm}%")))
                    .ToList();

                var mappedWerkzeuge = filteredWerkzeuge.Select(w => new Werkzeug
                {
                    WerkzeugId = w.WerkzeugId,
                    Marke = w.Marke,
                    Art = w.Art,
                    ProjektAdresse = w.ProjektAdresse,
                    Beschreibung = w.Beschreibung
                }).ToList();

                FilteredWerkzeuge = new ObservableCollection<Werkzeug>(mappedWerkzeuge);
                OnPropertyChanged(nameof(FilteredWerkzeuge));
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

        public ICommand SelectProjektCommand { get; private set; }
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
