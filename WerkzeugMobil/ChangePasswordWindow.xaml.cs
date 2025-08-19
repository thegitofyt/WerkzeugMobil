using Microsoft.Data.Sqlite;
using System;
using System.Windows;

namespace WerkzeugMobil
{
    public partial class ChangePasswordWindow : Window
    {
        private string _username;

        public ChangePasswordWindow(string username)
        {
            InitializeComponent();
            _username = username;
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string newPassword = NewPasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            if (string.IsNullOrWhiteSpace(newPassword) || newPassword != confirmPassword)
            {
                MessageBox.Show("Passwörter stimmen nicht überein oder sind leer.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            string hashed = BCrypt.Net.BCrypt.HashPassword(newPassword);

            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string dbDirectory = System.IO.Path.Combine(localAppData, "WerkzeugMobil");
            string dbPath = System.IO.Path.Combine(localAppData, "WerkzeugMobil", "WerkzeugMobilDb.sqlite");
            string connectionString = $"Data Source={dbPath};";

            try
            {
                using (var conn = new SqliteConnection(connectionString))
                {
                    conn.Open();
                    var updateCmd = new SqliteCommand("UPDATE Benutzer SET Passwort = @Passwort WHERE Benutzername = @Benutzername", conn);
                    updateCmd.Parameters.AddWithValue("@Passwort", hashed);
                    updateCmd.Parameters.AddWithValue("@Benutzername", _username);
                    updateCmd.ExecuteNonQuery();
                }

                MessageBox.Show("Passwort erfolgreich geändert.", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Fehler beim Aktualisieren des Passworts:\n" + ex.Message, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}