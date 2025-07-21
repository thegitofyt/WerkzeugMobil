using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Viewmodel;

namespace WerkzeugMobil
{
    public partial class AddProjekt : Window
    {
        public AddProjekt()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ListView_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {

        }

        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginUser();
            Application.Current.MainWindow = login; // Set the main window to the login window
            login.Show();
            this.Close();

            MessageBox.Show("Logout gedrückt");

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

        private void NavigateAddWerkzeug(object sender, RoutedEventArgs e)
        {
            var addProject = new AddWerkzeug(); // make sure this exists
            Application.Current.MainWindow = addProject; // Set the main window to the AddWerkzeug window
            addProject.Show();
            this.Close();
        }

        private void ToggleTheme_Click(object sender, RoutedEventArgs e)
        {
            if (App.Current.Resources.MergedDictionaries.FirstOrDefault(x => x.Source != null && x.Source.OriginalString.Contains("DarkTheme.xaml")) is ResourceDictionary darkTheme)
            {
                App.Current.Resources.MergedDictionaries.Remove(darkTheme);
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Themes/LightTheme.xaml", UriKind.Relative) });
            }
            else
            {
                App.Current.Resources.MergedDictionaries.Clear();
                App.Current.Resources.MergedDictionaries.Add(new ResourceDictionary { Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative) });
            }
        }
    }
}
