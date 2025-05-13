﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WerkzeugMobil.Data;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;
using WerkzeugMobil.MVVM.Viewmodel;
using Microsoft.Data.Sqlite;


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

        //private void ForgotPasswordClick(object sender, MouseButtonEventArgs e)
        //{
        //    PasswordResetWindow resetWindow = new PasswordResetWindow();
        //    resetWindow.ShowDialog(); // Öffnet das Fenster modal
        //}

        //private bool UserExists(string benutzername)
        //{
        //    string connectionString = "WerkzeugMobilDb.sqlite";
        //    using (SqliteConnection conn = new SqliteConnection(connectionString))
        //    {
        //        conn.Open();
        //        string query = "SELECT COUNT(*) FROM Users WHERE Benutzername = @Benutzername";
        //        using (SqliteCommand cmd = new SqliteCommand(query, conn))
        //        {
        //            cmd.Parameters.AddWithValue("@Benutzername", benutzername);
        //            long count = (long)cmd.ExecuteScalar();
        //            return count > 0;
        //        }
        //    }
        //}

        private void ForgotPasswordClick(object sender, MouseButtonEventArgs e)
        {
            string benutzername = BenutzernameInput.Text.Trim();

            if (string.IsNullOrEmpty(benutzername))
            {
                MessageBox.Show("Bitte geben Sie zuerst einen Benutzernamen ein.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Optional: prüfen, ob Benutzer existiert
            //if (!UserExists(benutzername))
            //{
            //    MessageBox.Show("Benutzername nicht gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            //    return;
            //}

            // Fenster öffnen und Benutzernamen übergeben
            PasswordResetWindow resetWindow = new PasswordResetWindow(benutzername);
            resetWindow.ShowDialog();
        }


        //private string GenerateTemporaryPassword()
        //{
        //    const string zeichen = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
        //    Random rnd = new Random();
        //    return new string(Enumerable.Repeat(zeichen, 10)
        //        .Select(s => s[rnd.Next(s.Length)]).ToArray());
        //}
        //private bool UpdateUserPasswordInDatabase(string benutzername, string neuesPasswort)
        //{
        //    // Hier müsstest du deine echte Datenbankabfrage einbauen.
        //    // Beispiel: UPDATE Users SET Passwort = HASH(neuesPasswort) WHERE Benutzername = benutzername

        //    return true; // Nur zum Testen, anpassen!
        //}


        //private void ForgotPasswordClick(object sender, MouseButtonEventArgs e)
        //{
        //    string benutzername = BenutzernameInput.Text;

        //    if (string.IsNullOrEmpty(benutzername))
        //    {
        //        MessageBox.Show("Bitte geben Sie zuerst Ihren Benutzernamen ein.");
        //        return;
        //    }

        //    // Temporäres Passwort generieren
        //    string neuesPasswort = GenerateTemporaryPassword();

        //    // Benutzer-Passwort in der Datenbank aktualisieren (nur Beispiel – anpassen!)
        //    bool updateErfolgreich = UpdateUserPasswordInDatabase(benutzername, neuesPasswort);

        //    if (!updateErfolgreich)
        //    {
        //        MessageBox.Show("Benutzer nicht gefunden oder Fehler beim Zurücksetzen.");
        //        return;
        //    }

        //    // E-Mail versenden
        //    try
        //    {
        //        MailMessage mail = new MailMessage("layal.shegaa23@gmail.com", "layal.shegaa07@email.com");
        //        mail.Subject = "Passwort zurücksetzen";
        //        mail.Body = $"Ihr temporäres Passwort lautet: {neuesPasswort}";

        //        SmtpClient smtpClient = new SmtpClient("smtp.deinserver.de", 587);
        //        smtpClient.Credentials = new NetworkCredential("layal.shegaa23@gmail.com", "");
        //        smtpClient.EnableSsl = true;

        //        smtpClient.Send(mail);

        //        MessageBox.Show("Ein temporäres Passwort wurde per E-Mail gesendet.");
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show("Fehler beim Senden der E-Mail: " + ex.Message);
        //    }
        //}


    }

}


