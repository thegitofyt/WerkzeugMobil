using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using WerkzeugMobil.DTO;

namespace WerkzeugMobil
{
    public partial class ProjektDetailsView : Window
    {
        public ProjektDetailsView(ProjektDTO projekt)
        {
            InitializeComponent();
            DataContext = projekt; // Bind project details
        }
    }
}
