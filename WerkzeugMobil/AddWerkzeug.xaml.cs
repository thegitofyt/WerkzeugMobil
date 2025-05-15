using System;
using System.Collections.Generic;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WerkzeugMobil.Data;
using WerkzeugMobil.MVVM.Model;
using WerkzeugMobil.MVVM.Viewmodel;

namespace WerkzeugMobil
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class AddWerkzeug : Window
    {
        private AddProjekt addProjekt; // Declare AddProjekt as a class member to keep track of it

        public AddWerkzeug()
        {
            //InitializeComponent();
            //DataContext = new AddWerkzeugViewModel();
            InitializeComponent();
            var viewModel = new AddWerkzeugViewModel();
            this.DataContext = viewModel;

            this.WindowState = WindowState.Maximized;

            addProjekt = new AddProjekt();  // Initialize the AddProjekt window
        }



        private void NavigateWerkzeug(object sender, RoutedEventArgs e)
        {
            // If the AddWerkzeug window is not open, show it
            if (!this.IsVisible)
            {
                this.Show(); // Show the AddWerkzeug window
            }

            // If the AddProjekt window is open, close it
            if (addProjekt.IsVisible)
            {
                addProjekt.Close();
            }
        }

        //private void UpdateButton_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        using (var context = new WerkzeugDbContext())
        //        {
        //            // Ensure Werkzeug is properly referenced from the DataContext  
        //            var viewModel = this.DataContext as AddWerkzeugViewModel;
        //            if (viewModel?.Tools == null)
        //            {
        //                MessageBox.Show("Kein Werkzeug zum Aktualisieren gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
        //                return;
        //            }

        //            var werkzeugInDb = context.Werkzeuge.FirstOrDefault(w => w.WerkzeugId == viewModel.Werkzeug.WerkzeugId);
        //            if (werkzeugInDb != null)
        //            {
        //                // Update values  
        //                werkzeugInDb.Marke = viewModel.Werkzeug.Marke;
        //                werkzeugInDb.Art = viewModel.Werkzeug.Art;
        //                werkzeugInDb.ProjektAdresse = viewModel.Werkzeug.ProjektAdresse;
        //                werkzeugInDb.Beschreibung = viewModel.Werkzeug.Beschreibung;

        //                context.SaveChanges();
        //                MessageBox.Show("Werkzeug erfolgreich aktualisiert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
        //            }
        //            else
        //            {
        //                MessageBox.Show("Werkzeug nicht gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show($"Fehler beim Aktualisieren des Werkzeugs: {ex.Message}", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}



        public AddWerkzeug(Werkzeug selectedWerkzeug)
        {
            InitializeComponent();

            // ViewModel erstellen und das übergebene Werkzeug zuweisen
            var viewModel = new AddWerkzeugViewModel();
            viewModel.Werkzeug = selectedWerkzeug; // Werkzeug an ViewModel übergeben
            DataContext = viewModel;
        }

        private void NavigateProjekt(object sender, RoutedEventArgs e)
        {
            // If the AddProjekt window is not open, show it
            if (!addProjekt.IsVisible)
            {
                addProjekt.Show(); // Show the AddProjekt window
            }

            // If the AddWerkzeug window is open, close it
            if (this.IsVisible)
            {
                this.Close();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var projekteView = new ProjekteView();
            projekteView.Show();
            this.Close(); // Schließt das aktuelle Fenster
        }
    }
}