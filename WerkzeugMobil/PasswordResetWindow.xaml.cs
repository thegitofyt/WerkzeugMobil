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
using System.Diagnostics;

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
            string dbPath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "WerkzeugMobilDb.sqlite");
            string connectionString = $"Data Source={dbPath};";
            ShowAllUsernames();
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

        private void ShowAllUsernames()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dbDirectory = System.IO.Path.Combine(localAppData, "WerkzeugMobil");
            string dbPath = System.IO.Path.Combine(localAppData, "WerkzeugMobil", "WerkzeugMobilDb.sqlite");
            string connectionString = $"Data Source={dbPath};";

            try
            {
                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = "SELECT Benutzername FROM Benutzer";

                    var reader = cmd.ExecuteReader();
                    StringBuilder sb = new StringBuilder("Gefundene Benutzer:\n");

                    while (reader.Read())
                    {
                        sb.AppendLine(reader.GetString(0));
                    }

                    MessageBox.Show(sb.ToString(), "Benutzerliste", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Lesen der Benutzernamen:\n" + ex.Message);
            }
        }

        private void SendResetLink(object sender, RoutedEventArgs e)
        {
            string empfaengerEmail = EmailInput.Text.Trim();

            if (string.IsNullOrEmpty(empfaengerEmail))
            {
                MessageBox.Show("Bitte gib eine gültige E-Mail-Adresse ein.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                string dbDirectory = System.IO.Path.Combine(localAppData, "WerkzeugMobil");
                string dbPath = System.IO.Path.Combine(localAppData, "WerkzeugMobil", "WerkzeugMobilDb.sqlite");
                string connectionString = $"Data Source={dbPath};";

                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();

                    // Check if user exists
                    var checkCmd = new SqliteCommand("SELECT COUNT(*) FROM Benutzer WHERE Benutzername = @Benutzername", conn);
                    checkCmd.Parameters.AddWithValue("@Benutzername", _benutzername);
                    var count = Convert.ToInt32(checkCmd.ExecuteScalar());
                    if (count == 0)
                    {
                        MessageBox.Show("Benutzername existiert nicht.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Generate temp password
                    string tempPassword = GenerateTemporaryPassword();
                    string hashed = BCrypt.Net.BCrypt.HashPassword(tempPassword);

                    // Update password in DB
                    var updateCmd = new SqliteCommand("UPDATE Benutzer SET Passwort = @Passwort WHERE Benutzername = @Benutzername", conn);
                    updateCmd.Parameters.AddWithValue("@Passwort", hashed);
                    updateCmd.Parameters.AddWithValue("@Benutzername", _benutzername);
                    updateCmd.ExecuteNonQuery();
                    int rowsAffected = updateCmd.ExecuteNonQuery();
                    Debug.WriteLine($"Rows affected by password update: {rowsAffected}");
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Update fehlgeschlagen - Benutzername existiert nicht.", "Fehler");
                    }
                    else
                    {
                        Debug.WriteLine("Password update successful.");

                        // Immediately verify
                        var verifyCmd = new SqliteCommand("SELECT Passwort FROM Benutzer WHERE Benutzername = @Benutzername", conn);
                        verifyCmd.Parameters.AddWithValue("@Benutzername", _benutzername);
                        string updatedPassword = (string)verifyCmd.ExecuteScalar();
                        Debug.WriteLine($"Updated password hash in DB: {updatedPassword}");

                        MessageBox.Show("Passwort erfolgreich aktualisiert.", "Erfolg");
                    }
                    // Send email
                    string absenderEmail = "layal.shegaa23@gmail.com";
                    string absenderPasswort = "guhr ownr ugdc lwkm"; // App-spezifisches Passwort

                    MailMessage nachricht = new MailMessage();
                    nachricht.From = new MailAddress(absenderEmail);
                    nachricht.To.Add(empfaengerEmail);
                    nachricht.Subject = "Temporäres Passwort für dein Konto";
                    nachricht.Body = $"Hallo {_benutzername},\n\nHier ist dein temporäres Passwort: {tempPassword}\nBitte ändere es nach dem Login.";

                    SmtpClient client = new SmtpClient("smtp.gmail.com", 587);
                    client.Credentials = new NetworkCredential(absenderEmail, absenderPasswort);
                    client.EnableSsl = true;

                    client.Send(nachricht);

                    MessageBox.Show("Temporäres Passwort gesendet!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                    this.Close();
                }
                  App.IsTemporaryLogin = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Senden der E-Mail:\n" + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GenerateTemporaryPassword()
        {
            // 8-character alphanumeric password
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var rand = new Random();
            return new string(Enumerable.Repeat(chars, 8)
                .Select(s => s[rand.Next(s.Length)]).ToArray());
        }


    }
}