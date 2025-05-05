using ListDemo.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Windows;
using System.Windows.Input;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;


namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class ProjekteViewModel : BaseViewModel
    {
        private ProjektDTO _selectedProjekt;
       
        private string _searchText;
       
        public ICommand OpenProjektCommand { get; }
        public ICommand FinishProjektCommand { get; }
        public ICommand SearchCommand { get; }
        private MainViewModel _mainViewModel;
        private ObservableCollection<ProjektDTO> _everyProjekt;
        private ObservableCollection<ProjektDTO> _allProjekte;
    

        // Property to set MainViewModel
        public MainViewModel MainViewModel
        {
            get { return _mainViewModel; }
            set
            {
                _mainViewModel = value;
                OnPropertyChanged(nameof(MainViewModel));
            }
        }

        public ObservableCollection<ProjektDTO> Projekte
        {
            get => _allProjekte;
            set
            {
                if (_allProjekte != value)  // Ensure only unique changes trigger the update
                {
                    _allProjekte = value;
                    OnPropertyChanged(nameof(Projekte));
                }
            }
        }
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                if (_searchText != value)
                {
                    _searchText = value;
                    OnPropertyChanged(nameof(SearchText));
                    FilterProjects(); // Automatically filter when the text changes
                }
            }
        }
        public ProjektDTO SelectedProjekt
        {
            get { return _selectedProjekt; }
            set
            {
                if (_selectedProjekt != value)
                {
                    _selectedProjekt = value;
                    OnPropertyChanged(nameof(SelectedProjekt));
                }
            }
        }

        // ICommand for selecting a project
       

        // Can execute logic for the command
        private bool CanExecuteSelectProjektCommand()
        {
            return SelectedProjekt != null; // Only allow command execution if a project is selected
        }

        // Execute logic for selecting a project
        private void ExecuteSelectProjektCommand()
        {
            if (SelectedProjekt != null)
            {
                // Call the LoadWerkzeugeForProject method from MainViewModel
                _mainViewModel.LoadWerkzeugeForProject(SelectedProjekt);
            }
        }

        public ProjekteViewModel()
        {

            
                LoadProjects();
            


            OpenProjektCommand = new RelayCommand(OpenProjekt);
            FinishProjektCommand = new RelayCommand(FinishProjekt, CanFinishProjekt);
            SearchCommand = new RelayCommand(SearchProjects);
           


        }


        private void LoadProjects()
        {
            try
            {
                using (var context = new WerkzeugDbContext())
                {
                    var projekte = context.Projekte.ToList();
                    _everyProjekt= new ObservableCollection<ProjektDTO>(projekte);
                    _allProjekte = new ObservableCollection<ProjektDTO>(projekte);
                    Projekte = new ObservableCollection<ProjektDTO>(_allProjekte);
                    OnPropertyChanged(nameof(Projekte));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Projekte: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void OpenProjekt(object parameter)
        {
            if (parameter is ProjektDTO projekt)
            {
                SelectedProjekt = projekt; // Set the selected project
                MessageBox.Show($"Selected Project: {SelectedProjekt.ProjektAddresse}");
            }
            if (SelectedProjekt != null)
            {
               
                MainNavigation mainNavigation = new MainNavigation();
                MainViewModel = new MainViewModel();
                MainViewModel.LoadWerkzeugeForProject(SelectedProjekt);
                mainNavigation.Show();
                var currentWindow = Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w.DataContext == this);
                currentWindow?.Close();

            }
            else
            {
                MessageBox.Show("Please select a project first.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        public void SetMainViewModel(MainViewModel mainViewModel)
        {
            MainViewModel = mainViewModel;
        }
        private bool CanFinishProjekt(object parameter)
        {
            return _selectedProjekt != null && _selectedProjekt.Werkzeuge.Any();
        }

        private void FinishProjekt(object parameter)
        {
            if (_selectedProjekt != null)
            {
                _selectedProjekt.Werkzeuge.Clear();
                MessageBox.Show("Alle Werkzeuge wurden ins Lager verschoben.", "Projekt abgeschlossen", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void SearchProjects(object parameter)
        {
            FilterProjects();
        }

        private void FilterProjects()
        {
            if (string.IsNullOrEmpty(SearchText))
            {
                Projekte = new ObservableCollection<ProjektDTO>(_everyProjekt);
            }
            else
            {
                using (var context = new WerkzeugDbContext())
                {
                    var filteredProjects = context.Projekte
                        .Include(p => p.Werkzeuge)
                        .Where(p => p.ProjektAddresse.ToLower().Contains(SearchText.ToLower())) // Case-insensitive search
                        .ToList();

                    Projekte = new ObservableCollection<ProjektDTO>(filteredProjects);
                    OnPropertyChanged(nameof(Projekte));
                }
            }
        }
    }
}
