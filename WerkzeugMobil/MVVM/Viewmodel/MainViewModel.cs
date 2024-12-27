using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using WerkzeugMobil.MVVM.Model;
using WerkzeugMobil.MVVM.Viewmodel;
using WerkzeugMobil.Services;


namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private readonly WerkzeugServices _werkzeugService;
        private Werkzeug _selectedWerkzeug;

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

        public MainViewModel()
        {
           
            _werkzeugService = new WerkzeugServices();
            Werkzeuge = new ObservableCollection<Werkzeug>(_werkzeugService.GetAllWerkzeuge());
#pragma warning disable IDE0028 // Initialisierung der Sammlung vereinfachen
            History = new ObservableCollection<string>();
#pragma warning restore IDE0028 // Initialisierung der Sammlung vereinfachen
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

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
