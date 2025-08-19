using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using WerkzeugMobil ;
using WerkzeugShared.DTO;
using WerkzeugShared.MVVM.Model;
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
            Debug.WriteLine("LoginUser window initialized");
        }

        private void CloseWindow(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BenutzernameInput_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
        }

        private void LoginButtonClick(object sender, RoutedEventArgs e)
        {
            string username = BenutzernameInput.Text;
            string password = PasswortInput.Password;

            if (AuthenticateUser(username, password))
            {
                if (RememberMeCheckbox.IsChecked == true)
                    SaveCredentials(username, password);
                else
                    ClearSavedCredentials();

                MessageBox.Show("Login erfolgreich!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

                if (App.IsTemporaryLogin)
                {
                    ChangePasswordWindow changeWindow = new ChangePasswordWindow(username);
                    changeWindow.ShowDialog();
                    App.IsTemporaryLogin = false;
                }

                ProjekteView projekteView = new ProjekteView();
                Application.Current.MainWindow = projekteView;
                projekteView.Show();
                this.Close();
            }
            else
            {
                MessageBox.Show("Ungültige Anmeldedaten!", "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool AuthenticateUser(string username, string password)
        {
            using (var context =  CreateDbContext())
            {
                var user = context.Benutzer.FirstOrDefault(u => u.Benutzername == username);
                if (user == null) return false;

                return BCrypt.Net.BCrypt.Verify(password, user.Passwort);
            }
        }

        private void SaveCredentials(string username, string password)
        {
            Properties.Settings.Default.SavedUsername = username;
            Properties.Settings.Default.SavedPassword = password;
            Properties.Settings.Default.RememberMe = true;
            Properties.Settings.Default.Save();
        }

        private void ClearSavedCredentials()
        {
            Properties.Settings.Default.SavedUsername = "";
            Properties.Settings.Default.RememberMe = false;
            Properties.Settings.Default.Save();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (Properties.Settings.Default.RememberMe)
            {
                BenutzernameInput.Text = Properties.Settings.Default.SavedUsername;
                PasswortInput.Password = Properties.Settings.Default.SavedPassword;
                RememberMeCheckbox.IsChecked = true;
            }
        }
        private WerkzeugShared.WerkzeugDbContext CreateDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<WerkzeugShared.WerkzeugDbContext>();

            // Adjust this path to your actual SQLite DB location
            var localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            var dbDirectory = System.IO.Path.Combine(localAppData, "WerkzeugMobil");
            var dbPath = System.IO.Path.Combine(dbDirectory, "WerkzeugMobilDb.sqlite");

            optionsBuilder.UseSqlite($"Data Source={dbPath}");

            return new WerkzeugShared.WerkzeugDbContext(optionsBuilder.Options);
        }
        private void ForgotPasswordClick(object sender, MouseButtonEventArgs e)
        {
            string benutzername = BenutzernameInput.Text.Trim();

            if (string.IsNullOrEmpty(benutzername))
            {
                MessageBox.Show("Bitte geben Sie zuerst einen Benutzernamen ein.", "Hinweis", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            PasswordResetWindow resetWindow = new PasswordResetWindow(benutzername);
            resetWindow.ShowDialog();
        }
    }
}