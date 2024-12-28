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
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

   

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            // Validate inputs
            if (string.IsNullOrWhiteSpace(BenutzernameInput.Text) || string.IsNullOrWhiteSpace(PasswortInput.Password))
            {
                MessageBox.Show("Bitte geben Sie sowohl Benutzername als auch Passwort ein.", "Eingabefehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create new Benutzer from inputs
            NewUser = new Benutzer
            {
                Benutzername = BenutzernameInput.Text,
                Passwort = PasswortInput.Password, // Use the Password property to get the entered password
                KannBearbeiten = true // Assuming this is a default value
            }; 

           
            MainNavigation mainNavigation = new MainNavigation();
            mainNavigation.Show(); // Show the MainNavigation window
            this.Close(); // Close the LoginUser  window
           
            
        }

        private void BenutzernameInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
    }
}
