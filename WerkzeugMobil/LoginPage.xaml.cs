using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;
using WerkzeugMobil.MVVM.Viewmodel;


namespace WerkzeugMobil
{
    public partial class LoginUser : Window
    {
        public Benutzer NewUser { get; private set; }

        private readonly List<UserDTO> benutzerListe;


        public LoginUser()
        {
            InitializeComponent();


        }
        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }



        //// private void LoginButtonClick(object sender, RoutedEventArgs e)
        //// {
        //     // Validate inputs
        //  //   if (string.IsNullOrWhiteSpace(BenutzernameInput.Text) || string.IsNullOrWhiteSpace(PasswortInput.Password))
        //  //   {
        //    ////''     MessageBox.Show("Bitte geben Sie sowohl Benutzername als auch Passwort ein.", "Eingabefehler", MessageBoxButton.OK, MessageBoxImage.Warning);
        //    //     return;
        //    // }

        //     // Create new Benutzer from inputs
        //     NewUser = new Benutzer
        //     {
        //         Benutzername = BenutzernameInput.Text,
        //         Passwort = PasswortInput.Password, // Use the Password property to get the entered password
        //         KannBearbeiten = true // Assuming this is a default value
        //     }; 


        //    MainNavigation mainNavigation = new MainNavigation();
        //    mainNavigation.Show(); // Show the MainNavigation window
        //    this.Close(); // Close the LoginUser  window


        //}

        private void BenutzernameInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {

        }
        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            string username = BenutzernameInput.Text;
            string password = PasswortInput.Password;

            using (var context = new WerkzeugDbContext())
            {
                // Überprüfen, ob Benutzername und Passwort in der Datenbank vorhanden sind
                var user = context.Benutzer
                    .FirstOrDefault(u => u.Benutzername == username && u.Passwort == password);

                if (user != null)
                {
                    // Login erfolgreich
                    MessageBox.Show("Login erfolgreich!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Navigiere zur Hauptseite
                    ProjekteView projekteView = new ProjekteView();
                    projekteView.Show();
                    this.Close(); // Schließt das Login-Fenster
                }
                else
                {
                    // Fehlgeschlagener Login
                    MessageBox.Show("Ungültige Anmeldedaten!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                if (AuthenticateUser(username, password))
                {
                    if (RememberMeCheckbox.IsChecked == true)
                    {
                        SaveCredentials(username, password);
                    }

                    MessageBox.Show("Login erfolgreich!");
                    this.Close();
                }

            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            // Authentifizierungslogik hier
            return username == "admin" && password == "password"; // Beispiel
        }

        private void SaveCredentials(string username, string password)
        {
            // Speicherung in Konfigurationsdatei
        }

        private void ForgotPasswordClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Benachrichtigen sie den Administrator.");
        }
    }

}


