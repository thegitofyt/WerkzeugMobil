using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WerkzeugShared.MVVM.Model;
using System.Windows;
using WerkzeugShared.DTO;
using WerkzeugShared.Services;
using System.Collections.ObjectModel;

using Microsoft.EntityFrameworkCore;

namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class AddProjektViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public Projekt Projekt { get; set; }
        private readonly ProjektServices _projektService;
        public ICommand SubmitCommand { get; }
        public ICommand NavigateWerkzeugCommand { get; }

        public ICommand DeleteProjektCommand { get; private set; }

        private ProjektDTO _selectedProjekt;
        public ProjekteViewModel projekteViewModel;

        public AddProjektViewModel()
        {
            _projektService = new ProjektServices(); // Initialisiere Service!
            projekteViewModel = new ProjekteViewModel();

            Projekt = new Projekt();
            SubmitCommand = new RelayCommand(Submit);
            NavigateWerkzeugCommand = new RelayCommand(NavigateToWerkzeug);
            DeleteProjektCommand = new RelayCommand(DeleteProjekt);

        }

        public ObservableCollection<ProjektDTO> Projekte
        {
            get => projekteViewModel.Projekte;
            set
            {
                if (projekteViewModel.Projekte != value)
                {
                    projekteViewModel.Projekte = value;
                    OnPropertyChanged(nameof(Projekte));
                }
            }
        }
        private async void DeleteProjekt()
        {
            if (SelectedProjekt == null)
            {
                MessageBox.Show("Bitte wählen Sie ein Projekt zum Löschen aus.", "Warnung", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                using (var context = CreateDbContext())
                {
                    var projektToDelete = context.Projekte
                        .FirstOrDefault(p => p.ProjektAddresse == SelectedProjekt.ProjektAddresse);

                    if (projektToDelete != null)
                    {
                        context.Projekte.Remove(projektToDelete);
                        int changes = context.SaveChanges();

                        if (changes > 0)
                        {
                            Projekte.Remove(SelectedProjekt);
                            SelectedProjekt = null;
                            context.SaveChanges();
                            await RefreshProjekteFromApiAsync();
                            MessageBox.Show("Projekt erfolgreich gelöscht.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            MessageBox.Show("Das Projekt konnte nicht gelöscht werden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    else
                    {
                        MessageBox.Show("Projekt nicht gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Löschen des Projekts: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        public ProjektDTO SelectedProjekt
        {
            get => _selectedProjekt;
            set
            {
                if (_selectedProjekt != value)
                {
                    _selectedProjekt = value;
                    OnPropertyChanged(nameof(SelectedProjekt));

                    if (_selectedProjekt != null)
                    {
                        Projekt.ProjektAddresse = _selectedProjekt.ProjektAddresse;
                        OnPropertyChanged(nameof(Projekt));
                    }

                    // Raise CanExecuteChanged for DeleteProjektCommand
                    ((RelayCommand)DeleteProjektCommand).RaiseCanExecuteChanged();
                }
            }
        }

        private async void Submit()
        {
            try
            {
                var projektDto = new ProjektDTO
                {
                    ProjektAddresse = Projekt.ProjektAddresse
                };

                _projektService.AddProjekt(projektDto);

                await RefreshProjekteFromApiAsync();
                // 🆕 Nach dem Hinzufügen Liste neu laden!
                projekteViewModel.LoadProjects();
                OnPropertyChanged(nameof(Projekte));

                MessageBox.Show("Projekt erfolgreich hinzugefügt!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                AddNew(); // Formular zurücksetzen
            }
            catch (Exception ex)
            {
                var errorMessage = $"Fehler beim Hinzufügen des Projekts: {ex.Message}";
                if (ex.InnerException != null)
                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";

                MessageBox.Show(errorMessage, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private async Task RefreshProjekteFromApiAsync()
        {
            try
            {
                var api = new WerkzeugApiService();
                var neueProjekte = await api.GetProjekteAsync();

                Projekte.Clear();
                foreach (var projekt in neueProjekte)
                {
                    Projekte.Add(projekt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Fehler beim Laden der Projekte von der API: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
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
        private async void AddNew()
        {
            Projekt = new Projekt();
            OnPropertyChanged(nameof(Projekt));
            await RefreshProjekteFromApiAsync();
        }

        private void NavigateToWerkzeug()
        {
            new AddWerkzeug().Show();
            Application.Current.Windows[0]?.Close();
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
