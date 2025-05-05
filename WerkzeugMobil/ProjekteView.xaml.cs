using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows;
using System.Windows.Controls;
using WerkzeugMobil.MVVM.Viewmodel;

namespace WerkzeugMobil
{
    public partial class ProjekteView : Window
    {
        public ProjekteView()
        {
            InitializeComponent();
            DataContext = new ProjekteViewModel();
            this.WindowState = WindowState.Maximized;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
