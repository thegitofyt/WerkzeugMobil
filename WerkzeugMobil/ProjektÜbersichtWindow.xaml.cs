using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;

using System.Collections.ObjectModel;

namespace WerkzeugMobil
{
    public partial class ProjektWindow : Window
    {
        public ObservableCollection<Projekt> Projekte { get; set; }

        public ProjektWindow()
        {
            InitializeComponent();

            // Sample data for Projekte
            Projekte = new ObservableCollection<Projekt>
            {
                new Projekt { ProjektId = 1, ProjektName = "Bauprojekt A", Status = "Laufend" },
                new Projekt { ProjektId = 2, ProjektName = "Sanierung B", Status = "Abgeschlossen" }
            };

            ProjektListView.ItemsSource = Projekte;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ProjektListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }
    }

    public class Projekt
    {
        public int ProjektId { get; set; }
        public string ProjektName { get; set; }
        public string Status { get; set; }
    }
}
