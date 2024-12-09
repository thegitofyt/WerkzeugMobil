using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using WerkzeugMobil.MVVM.Model;
using WerkzeugMobil.MVVM.Viewmodel;


namespace WerkzeugMobil
{
    public partial class AddWerkzeugWindow : Window
    {
        public Werkzeug NewWerkzeug { get; private set; }

        public AddWerkzeugWindow()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Create new Werkzeug from inputs
            NewWerkzeug = new Werkzeug
            {
                WerkzeugNummer = WerkzeugNummerInput.Text,
                Marke = MarkeInput.Text,
                Art = ArtInput.Text,
                ProjektAdresse = ProjektAdresseInput.Text,
                Beschreibung = BeschreibungInput.Text,
                Lager = LagerInput.IsChecked ?? false
            };

            DialogResult = true;
            this.Close();
        }
    }
}
