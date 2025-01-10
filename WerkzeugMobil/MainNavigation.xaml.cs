using System.Collections.Generic;
using System.Linq;
using WerkzeugMobil.MVVM.Model;

using System.Windows;
using System.Windows.Navigation;
using WerkzeugMobil.MVVM.Viewmodel;

namespace WerkzeugMobil
{
    public partial class MainNavigation : Window
    {
        public MainNavigation()
        {
            InitializeComponent();
            DataContext = new MainViewModel();

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
        private void Maschine_Lager(object sender, RoutedEventArgs e)
        {
            Lager lager = new Lager();
            lager.Show(); // Show the MainNavigation window
            this.Close(); // Close the LoginUser  window
        }
    }
}
