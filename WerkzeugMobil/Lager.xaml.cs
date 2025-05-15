using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;
using WerkzeugMobil.MVVM.Viewmodel;

namespace WerkzeugMobil
{
    /// <summary>
    /// Interaktionslogik für Window1.xaml
    /// </summary>
    public partial class Lager : Window
    {
        public ObservableCollection<WerkzeugDto> Werkzeuge { get; set; }

        public Lager()
        {
            InitializeComponent();
            DataContext = new LagerViewModel();

            this.WindowState = WindowState.Maximized;
        }

        private void LoadWerkzeuge()
        {
            try
            {
                using (var context = new WerkzeugDbContext())
                {
                    // Fetch Werkzeuge from the database
                    var werkzeuge = context.Werkzeuge.ToList();
                    Werkzeuge = new ObservableCollection<WerkzeugDto>(werkzeuge);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Werkzeuge: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void WerkzeugDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var viewModel = DataContext as LagerViewModel;

            if (viewModel?.SelectedWerkzeug != null)
            {
                viewModel.NavigateToAddWerkzeug();

                // Optional: Auswahl zurücksetzen, damit man dasselbe Element nochmal klicken kann
                ((DataGrid)sender).SelectedItem = null;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var Mainnav = new MainNavigation();
            Mainnav.Show();
            this.Close(); // Schließt das aktuelle Fenster
        }

    }

}
