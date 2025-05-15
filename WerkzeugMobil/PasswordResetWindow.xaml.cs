using System;
using System.Collections.Generic;
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

using System.Windows;
using System.Net.Mail;
using System.Net;
using Microsoft.Data.Sqlite;

namespace WerkzeugMobil
{
    public partial class PasswordResetWindow : Window
    {
        public PasswordResetWindow()
        {
            InitializeComponent();
        }


        private string _benutzername;

        public PasswordResetWindow(string benutzername)
        {
            InitializeComponent();
            _benutzername = benutzername;

            // Beispiel: Zeige den Namen irgendwo
        }

        //private string GetPasswordFromDatabase(string benutzername)
        //    {
        //        string connectionString = "Data Source=deineDatenbank.db;Version=3;";
        //        string passwort = null;

        //        using (SQLiteConnection conn = new SQLiteConnection(connectionString))
        //        {
        //            conn.Open();
        //            string query = "SELECT Passwort FROM Users WHERE Benutzername = @Benutzername";

        //            using (SQLiteCommand cmd = new SQLiteCommand(query, conn))
        //            {
        //                cmd.Parameters.AddWithValue("@Benutzername", benutzername);

        //                object result = cmd.ExecuteScalar();
        //                if (result != null)
        //                {
        //                    passwort = result.ToString();
        //                }
        //            }
        //        }

        //    return passwort;
        //}

        //public string GetPassword(string benutzername)
        //{
        //    string connectionString = "Data Source=meineDatenbank.db;";
        //    string passwort = null;

        //    using (var conn = new SqliteConnection(connectionString))
        //    {
        //        conn.Open();
        //        var cmd = new SqliteCommand("SELECT Passwort FROM Users WHERE Benutzername = @Benutzername", conn);
        //        cmd.Parameters.AddWithValue("@Benutzername", benutzername);

        //        var result = cmd.ExecuteScalar();
        //        if (result != null)
        //        {
        //            passwort = result.ToString();
        //        }
        //    }

        //    return passwort;
        //}
        private string GetPasswordFromDatabase(string benutzername)
        {
            string connectionString = "Data Source=WerkzeugMobilDb.sqlite;";
            using (SqliteConnection conn = new SqliteConnection(connectionString))
            {
                conn.Open();
                string query = "SELECT Passwort FROM Benutzer WHERE Benutzername = @Benutzername";
                using (SqliteCommand cmd = new SqliteCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Benutzername", benutzername);
                    object result = cmd.ExecuteScalar();
                    return result != null ? result.ToString() : null;
                }
            }
        }
         

        //private void SendResetLink(object sender, RoutedEventArgs e)
        //{
        //    string empfaengerEmail = EmailInput.Text.Trim();

        //    if (string.IsNullOrEmpty(empfaengerEmail))
        //    {
        //        MessageBox.Show("Bitte gib eine gültige E-Mail-Adresse ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
        //        return;
        //    }

        //    try
        //    {
        //        // Absender-Daten (ersetzen!)
        //        string absenderEmail = "layal.shegaa23@gmail.com";  // <-- Ersetzen
        //        string absenderPasswort = "guhr ownr ugdc lwkm"; // <-- App-Passwort von Gmail

        //        MailMessage nachricht = new MailMessage();
        //        nachricht.From = new MailAddress(absenderEmail);
        //        nachricht.To.Add(empfaengerEmail);
        //        nachricht.Subject = "Passwort zurücksetzen";
        //        nachricht.Body = "Hier ist dein Passwort: " + GetPassword;

        //        SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
        //        client.Credentials = new NetworkCredential(absenderEmail, absenderPasswort);
        //        client.EnableSsl = true;

        //        client.Send(nachricht);

        //        MessageBox.Show("E-Mail erfolgreich gesendet!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
        //        this.Close();
        //    }
        //    catch (System.Exception ex)
        //    {
        //        MessageBox.Show("Fehler beim Senden der E-Mail:\n" + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
        //}

        private void SendResetLink(object sender, RoutedEventArgs e)
        {
            string empfaengerEmail = EmailInput.Text.Trim();

            if (string.IsNullOrEmpty(empfaengerEmail))
            {
                MessageBox.Show("Bitte gib eine gültige E-Mail-Adresse ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string benutzerPasswort = GetPasswordFromDatabase(_benutzername);

            if (string.IsNullOrEmpty(benutzerPasswort))
            {
                MessageBox.Show("Kein Passwort für diesen Benutzer gefunden.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                string absenderEmail = "layal.shegaa23@gmail.com";
                string absenderPasswort = "guhr ownr ugdc lwkm"; // App-spezifisches Passwort

                MailMessage nachricht = new MailMessage();
                nachricht.From = new MailAddress(absenderEmail);
                nachricht.To.Add(empfaengerEmail);
                nachricht.Subject = "Passwort zurücksetzen";
                nachricht.Body = $"Hallo {_benutzername},\n\nDein Passwort lautet: {benutzerPasswort}";

                SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                client.Credentials = new NetworkCredential(absenderEmail, absenderPasswort);
                client.EnableSsl = true;

                client.Send(nachricht);

                MessageBox.Show("E-Mail erfolgreich gesendet!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (System.Exception ex)
            {
                MessageBox.Show("Fehler beim Senden der E-Mail:\n" + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



    }
}