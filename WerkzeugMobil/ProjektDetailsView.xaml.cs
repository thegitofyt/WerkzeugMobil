using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Data;
using WerkzeugMobil.Converters;
using WerkzeugShared.DTO;
using WerkzeugMobil.MVVM.Viewmodel;

namespace WerkzeugMobil
{
    public partial class ProjektDetailsView : Window
    {
        public ProjekteViewModel ViewModel => DataContext as ProjekteViewModel;

        public ProjektDetailsView()
        {
            InitializeComponent();
            DataContext = new ProjekteViewModel();
          


        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
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

    }
}

