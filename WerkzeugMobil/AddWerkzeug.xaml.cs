using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using WerkzeugShared.DTO;
using WerkzeugMobil.MVVM.Viewmodel;

namespace WerkzeugMobil
{
    /// <summary>
    /// Interaction logic for AddWerkzeug.xaml
    /// </summary>
    public partial class AddWerkzeug : Window
    {
        private AddProjekt addProjekt;

        public AddWerkzeug()
        {
            InitializeComponent();

            var viewModel = new AddWerkzeugViewModel();
            this.DataContext = viewModel;

            this.WindowState = WindowState.Maximized;
            addProjekt = new AddProjekt(); // Optional: Only if you want to reuse it
        }


        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            ReloadMainNavigationWithSelectedProject();
        }

        private void NavigateDashboard(object sender, RoutedEventArgs e)
        {
            ReloadMainNavigationWithSelectedProject();
        }
        private void ReloadMainNavigationWithSelectedProject()
        {
            var proVie = new ProjekteView();
            var vm = proVie.DataContext as ProjekteViewModel;

            if (vm == null)
            {
                MessageBox.Show("ProjekteViewModel ist null!");
                return;
            }

            var selectedProjekt = App.Current.Properties["LastSelectedProjekt"] as ProjektDTO;
            if (selectedProjekt == null)
            {
                MessageBox.Show("Kein gespeichertes Projekt gefunden.");
                return;
            }

            vm.OpenProjekt(selectedProjekt);
        }

        private void NavigateLager(object sender, RoutedEventArgs e)
        {
            var lager = new Lager();
            Application.Current.MainWindow = lager; // Set the main window to the Lager window
            lager.Show();
            this.Close();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var login=new LoginUser();
            Application.Current.MainWindow = login; // Set the main window to the login window
            login.Show();
            this.Close();

            MessageBox.Show("Logout gedrückt");
            
        }
        private void NavigateAddProjekt(object sender, RoutedEventArgs e)
        {
            var addProjekt = new AddProjekt();
            Application.Current.MainWindow = addProjekt; // Set the main window to the AddProjekt window
            addProjekt.Show(); // Use the existing instance
            this.Close();
        }

        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            var lightTheme = App.Current.Resources.MergedDictionaries
                .FirstOrDefault(x => x.Source?.OriginalString.Contains("LightTheme.xaml") == true);
            var darkTheme = App.Current.Resources.MergedDictionaries
                .FirstOrDefault(x => x.Source?.OriginalString.Contains("DarkTheme.xaml") == true);

            if (lightTheme != null)
            {
                // Switch to Dark
                App.Current.Resources.MergedDictionaries.Remove(lightTheme);
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Theme/DarkTheme.xaml", UriKind.Relative) });
            }
            else if (darkTheme != null)
            {
                // Switch to Light
                App.Current.Resources.MergedDictionaries.Remove(darkTheme);
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Theme/LightTheme.xaml", UriKind.Relative) });
            }
            else
            {
                // If neither is present, add light as fallback
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Theme/LightTheme.xaml", UriKind.Relative) });
            }
        }

       
    }
}