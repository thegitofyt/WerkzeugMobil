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
            LoadWerkzeuge();
            DataContext = this; // Bind the DataContext to this class
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
    }

}
