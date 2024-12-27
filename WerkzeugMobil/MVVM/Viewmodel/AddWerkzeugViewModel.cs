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

namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class AddWerkzeugViewModel : INotifyPropertyChanged
    {
        private readonly WerkzeugServices _werkzeugService;

        public event PropertyChangedEventHandler? PropertyChanged;

        public Werkzeug Werkzeug { get; set; }

        public ICommand AddNewCommand { get; }
        public ICommand SubmitCommand { get; }

        public AddWerkzeugViewModel()
        {
            _werkzeugService = new WerkzeugServices();
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

            // Optionally reset the form or navigate away
            AddNew(); // Reset the form after submission
        }

        

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

 
