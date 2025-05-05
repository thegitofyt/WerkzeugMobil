using ListDemo.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;

namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class LagerViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<WerkzeugDto> Werkzeuge { get; set; }
        private ObservableCollection<WerkzeugDto> _allWerkzeuge;

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
            }
        }

        public ICommand FilterInLagerCommand { get; }
        public ICommand FilterMitAdresseCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ResetCommand { get; }

        public LagerViewModel()
        {
            LoadWerkzeugeFromDatabase();

            FilterInLagerCommand = new RelayCommand(FilterInLager);
            FilterMitAdresseCommand = new RelayCommand(FilterMitAdresse);
            SearchCommand = new RelayCommand(Search);
            ResetCommand = new RelayCommand(Reset);
        }

        private WerkzeugDto _selectedWerkzeug;

        public WerkzeugDto SelectedWerkzeug
        {
            get => _selectedWerkzeug;
            set
            {
                _selectedWerkzeug = value;
                OnPropertyChanged();
                NavigateToAddWerkzeug(); // Navigate when a Werkzeug is selected
            }
        }

        public void NavigateToAddWerkzeug()
        {
            if (SelectedWerkzeug == null)
            {
               MessageBox.Show("Bitte ein Werkzeug auswählen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                // If nothing is selected, create a new Add dialog
                var addWerkzeug = new AddWerkzeug();
                var addWerkzeugViewModel = new AddWerkzeugViewModel();  // Create new Werkzeug
                addWerkzeug.DataContext = addWerkzeugViewModel;
                addWerkzeug.Show();
            }
            else
            {
                // If an existing Werkzeug is selected, open it for editing
                var addWerkzeug = new AddWerkzeug();
                var addWerkzeugViewModel = new AddWerkzeugViewModel(SelectedWerkzeug); // Pass the DTO to the constructor
                addWerkzeug.DataContext = addWerkzeugViewModel;
                addWerkzeug.Show();
            }
        }

        
        private void LoadWerkzeugeFromDatabase()
        {
            try
            {
                using (var context = new WerkzeugDbContext())
                {
                    var werkzeuge = context.Werkzeuge.ToList();
                    _allWerkzeuge = new ObservableCollection<WerkzeugDto>(werkzeuge);
                    Werkzeuge = new ObservableCollection<WerkzeugDto>(_allWerkzeuge);
                    OnPropertyChanged(nameof(Werkzeuge));
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error loading Werkzeuge: {ex.Message}", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

        private void FilterInLager()
        {
            if (_allWerkzeuge == null || !_allWerkzeuge.Any()) return; // Prevent empty filter
            Werkzeuge = new ObservableCollection<WerkzeugDto>(_allWerkzeuge.Where(w => w.Lager));
            OnPropertyChanged(nameof(Werkzeuge));
        }

        private void FilterMitAdresse()
        {
            if (_allWerkzeuge == null || !_allWerkzeuge.Any()) return; // Prevent empty filter
            Werkzeuge = new ObservableCollection<WerkzeugDto>(_allWerkzeuge.Where(w => !string.IsNullOrEmpty(w.ProjektAdresse)));
            OnPropertyChanged(nameof(Werkzeuge));
        }

        private void Search()
        {
            if (!string.IsNullOrWhiteSpace(SearchText))
            {
                Werkzeuge = new ObservableCollection<WerkzeugDto>(_allWerkzeuge.Where(w =>
                    (w.WerkzeugId?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (w.Art?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false) ||
                    (w.Marke?.Contains(SearchText, StringComparison.OrdinalIgnoreCase) ?? false)));
                OnPropertyChanged(nameof(Werkzeuge));
            }
        }

        private void Reset()
        {
            Werkzeuge = new ObservableCollection<WerkzeugDto>(_allWerkzeuge);
            SearchText = string.Empty;
            OnPropertyChanged(nameof(Werkzeuge));
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
