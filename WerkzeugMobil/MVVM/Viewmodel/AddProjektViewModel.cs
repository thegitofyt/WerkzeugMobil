using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WerkzeugMobil.MVVM.Model;
using ListDemo.ViewModels;
using System.Windows;
using WerkzeugMobil.DTO;
using WerkzeugMobil.Services;
using System.Collections.ObjectModel;
using WerkzeugMobil.Data;

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
        private void DeleteProjekt(object parameter)
        {
            var projekt = parameter as ProjektDTO;

            if (projekt != null)
            {
                try
                {
                    using (var context = new WerkzeugDbContext())
                    {
                        // Suche das Projekt in der Datenbank
                        var projektToDelete = context.Projekte
                            .FirstOrDefault(p => p.ProjektAddresse == projekt.ProjektAddresse);

                        if (projektToDelete != null)
                        {
                            context.Projekte.Remove(projektToDelete); // Entferne das Projekt aus der DB
                            context.SaveChanges(); // Speichere die Änderungen
                            MessageBox.Show("Projekt erfolgreich gelöscht.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

                            // Entferne das Projekt aus der ObservableCollection (damit die UI aktualisiert wird)
                            Projekte.Remove(projekt);
                        }
                        else
                        {
                            MessageBox.Show("Projekt nicht gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Fehlerbehandlung
                    MessageBox.Show($"Fehler beim Löschen des Projekts: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Kein Projekt zum Löschen ausgewählt.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
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
                }
            }
        }

        private void Submit()
        {
            try
            {
                var projektDto = new ProjektDTO
                {
                    ProjektAddresse = Projekt.ProjektAddresse
                };

                _projektService.AddProjekt(projektDto);

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

        private void AddNew()
        {
            Projekt = new Projekt();
            OnPropertyChanged(nameof(Projekt));
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
