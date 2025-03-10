using ListDemo.ViewModels;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;

namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class ProjekteViewModel : BaseViewModel
    {
        private ProjektDTO _selectedProjekt;
        public ObservableCollection<ProjektDTO> Projekte { get; set; }
        public ICommand OpenProjektCommand { get; }
        public ICommand FinishProjektCommand { get; }

        public ProjekteViewModel()
        {
            Projekte = new ObservableCollection<ProjektDTO>
            {
                new ProjektDTO { ProjektAddresse = "Musterstraße 1", Werkzeuge = new List<WerkzeugDto>
                    {
                        new WerkzeugDto { WerkzeugId = "W001", Marke = "Bosch", Art = "Bohrmaschine" },
                        new WerkzeugDto { WerkzeugId = "W002", Marke = "Makita", Art = "Säge" }
                    }
                },
                new ProjektDTO { ProjektAddresse = "Beispielweg 23", Werkzeuge = new List<WerkzeugDto>
                    {
                        new WerkzeugDto { WerkzeugId = "W003", Marke = "DeWalt", Art = "Akkuschrauber" }
                    }
                }
            };

            OpenProjektCommand = new RelayCommand(OpenProjekt);
            FinishProjektCommand = new RelayCommand(FinishProjekt, CanFinishProjekt);
        }

        private void OpenProjekt(object parameter)
        {
            if (parameter is ProjektDTO projekt)
            {
                _selectedProjekt = projekt;
                var projektFenster = new ProjektDetailsView(projekt)
                {
                    DataContext = this
                };
                projektFenster.Show();
            }
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
    }
}
