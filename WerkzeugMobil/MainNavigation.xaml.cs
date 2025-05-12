using System.Collections.Generic;
using System.Linq;
using WerkzeugMobil.MVVM.Model;

using System.Windows;
using System.Windows.Navigation;
using WerkzeugMobil.MVVM.Viewmodel;
using WerkzeugMobil.DTO;

namespace WerkzeugMobil
{
    public partial class MainNavigation : Window
    {
        private ProjektDTO _projekt;
        public MainNavigation()
        {
            
            DataContext = new MainViewModel();
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            AddWerkzeug addWerkzeug = new AddWerkzeug();
            addWerkzeug.Show(); // Show the MainNavigation window
            this.Close(); // Close the LoginUser  window
        }

        private void Projekt_Click(object sender, RoutedEventArgs e)
        {
            ProjektDetailsView projektDetailsView = new ProjektDetailsView();
            projektDetailsView.Show(); // Show the MainNavigation window
            this.Close(); // Close the LoginUser  window
        }
        private void Maschine_Lager(object sender, RoutedEventArgs e)
        {
            Lager lager = new Lager();
            lager.Show(); // Show the Lager window
            this.Close(); // Close the MainNavigation  window
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var projekteView = new ProjekteView();
            projekteView.Show();
            this.Close(); // Schließt das aktuelle Fenster
        }

    }
}
