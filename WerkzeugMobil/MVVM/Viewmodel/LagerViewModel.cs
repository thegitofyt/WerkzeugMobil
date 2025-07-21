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
using System.Windows.Input;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;

using System.Diagnostics;
using System.Linq;
using WerkzeugMobil.Services;
using Microsoft.EntityFrameworkCore;


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
        private WerkzeugDto _selectedWerkzeug;
        public WerkzeugDto SelectedWerkzeug
        {
            get => _selectedWerkzeug;
            set
            {
                if (_selectedWerkzeug != value)
                {
                    _selectedWerkzeug = value;
                    OnPropertyChanged(nameof(SelectedWerkzeug));
                }
            }
        }

        public ICommand NavigateToAddWerkzeugCommand { get; }

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
            NavigateToAddWerkzeugCommand = new RelayCommand(NavigateToAddWerkzeug);
        }

        public void NavigateToAddWerkzeug()
        {
            if (SelectedWerkzeug != null)
            {
                // Konvertiere WerkzeugDto zu Werkzeug
                var werkzeug = new Werkzeug
                {
                    WerkzeugId = SelectedWerkzeug.WerkzeugId,
                    Marke = SelectedWerkzeug.Marke,
                    Art = SelectedWerkzeug.Art,
                    Beschreibung = SelectedWerkzeug.Beschreibung,
                    ProjektAdresse = SelectedWerkzeug.ProjektAdresse,
                    History = SelectedWerkzeug.History,
                    Lager = SelectedWerkzeug.Lager,
                    Projekt = SelectedWerkzeug.Projekt
                };

                // Erstelle das ViewModel und übergebe das Werkzeug
                var addWerkzeugViewModel = new AddWerkzeugViewModel();
                addWerkzeugViewModel.PopulateWerkzeug(werkzeug);

                // Erstelle das AddWerkzeug-Fenster und setze den DataContext
                var addWerkzeugWindow = new AddWerkzeug();
                addWerkzeugWindow.DataContext = addWerkzeugViewModel;

                // Zeige das neue Fenster an
                addWerkzeugWindow.Show();
            }
            else
            {
                MessageBox.Show("Bitte wähle zuerst ein Werkzeug aus.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
        private async Task RefreshWerkzeugeFromApiAsync()
        {
            try
            {
                var api = new WerkzeugApiService();
                var neueWerkzeuge = await api.GetWerkzeugeAsync();

                Werkzeuge.Clear();
                foreach (var werkzeug in neueWerkzeuge)
                {
                    Werkzeuge.Add(werkzeug);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Werkzeuge von der API: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private WerkzeugDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WerkzeugDbContext>();

            // Adjust path if needed - example using local app data folder
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbPath = System.IO.Path.Combine(localAppData, "WerkzeugMobil", "WerkzeugMobilDb.sqlite");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new WerkzeugDbContext(optionsBuilder.Options);
        }
        private async void LoadWerkzeugeFromDatabase()
        {
            try
            {
                using (var context = CreateDbContext())
                {
                    var werkzeuge = context.Werkzeuge.ToList();
                    _allWerkzeuge = new ObservableCollection<WerkzeugDto>(werkzeuge);
                    Werkzeuge = new ObservableCollection<WerkzeugDto>(_allWerkzeuge);
                    OnPropertyChanged(nameof(Werkzeuge));
                    await RefreshWerkzeugeFromApiAsync();
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
