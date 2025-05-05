using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WerkzeugMobil.MVVM.Model;
using ListDemo.ViewModels;
using System.Windows;
using WerkzeugMobil.DTO;
using WerkzeugMobil.Services;
using System.Collections.ObjectModel;
namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class AddProjektViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Projekt Projekt { get; set; }
        
        private readonly ProjektServices _projektService;
        public ICommand SubmitCommand { get; }
        public ICommand NavigateWerkzeugCommand { get; }
        private ProjektDTO _selectedProjekt;

        public ProjekteViewModel projekteViewModel;
        public AddProjektViewModel()

        {
            projekteViewModel = new ProjekteViewModel();
            Projekt = new Projekt();
            SubmitCommand = new RelayCommand(Submit);
            NavigateWerkzeugCommand = new RelayCommand(NavigateToWerkzeug);
        }
        public ObservableCollection<ProjektDTO> Projekte
        {
            get => projekteViewModel.Projekte;
            set
            {
                if (projekteViewModel.Projekte != value)  // Ensure only unique changes trigger the update
                {
                    projekteViewModel.Projekte = value;
                    OnPropertyChanged(nameof(Projekte));
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

                    // Ensure Projekt is updated when a new selection is made
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
                // Create a DTO from the Werkzeug model
                var projektDto = new ProjektDTO
                {
                    ProjektAddresse = Projekt.ProjektAddresse
                };

                // Add the new Werkzeug
                _projektService.AddProjekt(projektDto);

               
              

                // Success feedback
                MessageBox.Show("Werkzeug erfolgreich hinzugefügt!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

                // Optionally reset the form
                AddNew(); // Reset the form after submission
            }
            catch (Exception ex)
            {
                // Error feedback
                var errorMessage = $"Fehler beim Hinzufügen des Werkzeugs: {ex.Message}";
                if (ex.InnerException != null)
                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";

                MessageBox.Show(errorMessage, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddNew()
        {
           Projekt = new Projekt(); // Reset the form
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
