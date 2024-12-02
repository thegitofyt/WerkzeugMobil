using System.Windows;
using WerkzeugMobil.Services;
using WerkzeugMobil.MVVM.Model;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using WerkzeugMobil.MVVM.Viewmodel;
using System.Collections.ObjectModel;

namespace WerkzeugMobil
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Werkzeug> Werkzeuge { get; set; }
        public ObservableCollection<HistoryEntry> HistoryEntries { get; set; }

        public MainWindow()
        {
            InitializeComponent();

       
            Werkzeuge = new ObservableCollection<Werkzeug>
            {
                new Werkzeug { WerkzeugId = 1, WerkzeugName = "Hammer", Standort = "Lager", Status = "Verfügbar" },
                new Werkzeug { WerkzeugId = 2, WerkzeugName = "Bohrmaschine", Standort = "Baustelle", Status = "In Benutzung" }
            };

           
            HistoryEntries = new ObservableCollection<HistoryEntry>
            {
                new HistoryEntry { WerkzeugId = 1, WerkzeugName = "Hammer", Datum = "2024-11-30", Standort = "Lager" },
                new HistoryEntry { WerkzeugId = 2, WerkzeugName = "Bohrmaschine", Datum = "2024-11-29", Standort = "Baustelle" }
            };

            WerkzeugListView.ItemsSource = Werkzeuge;
            HistoryList.ItemsSource = HistoryEntries;
        }

        private void WerkzeugButton_Click(object sender, RoutedEventArgs e)
        {
            WerkzeugList.Visibility = Visibility.Visible;
            HistoryView.Visibility = Visibility.Collapsed;
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            WerkzeugList.Visibility = Visibility.Collapsed;
            HistoryView.Visibility = Visibility.Visible;
        }

        private void ProjektButton_Click(object sender, RoutedEventArgs e)
        {
            ProjektWindow projektWindow = new ProjektWindow();
            projektWindow.ShowDialog();
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            AddWerkzeugWindow addWerkzeugWindow = new AddWerkzeugWindow();

            if (addWerkzeugWindow.ShowDialog() == true)
            {
                // Add the new Werkzeug to the list
                Werkzeuge.Add(addWerkzeugWindow.NewWerkzeug);
            }
        }


        public class Werkzeug
    {
        public int WerkzeugId { get; set; }
        public string WerkzeugName { get; set; }
        public string Standort { get; set; }
        public string Status { get; set; }
    }

    public class HistoryEntry
    {
        public int WerkzeugId { get; set; }
        public string WerkzeugName { get; set; }
        public string Datum { get; set; }
        public string Standort { get; set; }
    }
}
