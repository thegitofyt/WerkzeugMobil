using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;
using WerkzeugMobil.Services;
using ListDemo.ViewModels;
using System.Windows;

namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class AddWerkzeugViewModel : INotifyPropertyChanged
    {
        private readonly WerkzeugServices _werkzeugService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Werkzeug Werkzeug { get; set; }

        public ICommand AddNewCommand { get; }
        public ICommand SubmitCommand { get; }
        public AddWerkzeugViewModel() : this(null) { }

        public AddWerkzeugViewModel(WerkzeugServices werkzeugService = null)
        {
            // Dependency Injection
            _werkzeugService = werkzeugService ?? new WerkzeugServices();
            Werkzeug = new Werkzeug();
            AddNewCommand = new RelayCommand(AddNew);
            SubmitCommand = new RelayCommand(Submit);
        }

        private void AddNew()
        {
            Werkzeug = new Werkzeug(); // Reset the form
            OnPropertyChanged(nameof(Werkzeug));
        }

        private void Submit()
        {
            try
            {
                // Create a DTO from the Werkzeug model
                var werkzeugDto = new WerkzeugDto
                {
                    WerkzeugId = Werkzeug.WerkzeugId,
                    Marke = Werkzeug.Marke,
                    Art = Werkzeug.Art,
                    ProjektAdresse = Werkzeug.ProjektAdresse,
                    Beschreibung = Werkzeug.Beschreibung
                };

                // Add the new Werkzeug
                _werkzeugService.AddWerkzeug(werkzeugDto);

                // Optionally, update the address history
                _werkzeugService.UpdateAddressHistory(Werkzeug.WerkzeugId, Werkzeug.ProjektAdresse);

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

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}


