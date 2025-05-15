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
    public partial class ProjektDetailsView : Window
    {
        public ProjektDetailsView()
        {
            InitializeComponent();
            DataContext = new ProjekteViewModel(); // Bind project details
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainnav = new MainNavigation();
            mainnav.Show();
            this.Close(); // Schließt das aktuelle Fenster
        }
    }
}
