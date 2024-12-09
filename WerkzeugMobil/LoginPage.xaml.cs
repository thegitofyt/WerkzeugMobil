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
    public partial class LoginUser : Window
    {
        public Benutzer NewUser { get; private set; }

        public LoginUser()
        {
            InitializeComponent();
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            // Create new Werkzeug from inputs
            NewUser = new Benutzer
            {
                Benutzername = BenutzernameInput.Text,
                Passwort = PasswortInput.Text,
                KannBearbeiten = true
            };

            DialogResult = true;
            this.Close();
        }
    }
}
