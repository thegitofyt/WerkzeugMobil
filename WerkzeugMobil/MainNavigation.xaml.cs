using ListDemo.ViewModels;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Viewmodel;

namespace WerkzeugMobil
{
    public partial class MainNavigation : Window, INotifyPropertyChanged
    {
        private ProjektDTO _projekt;
        private bool _isDarkMode;

        public bool IsDarkMode
        {
            get => _isDarkMode;
            set
            {
                if (_isDarkMode != value)
                {
                    _isDarkMode = value;
                    OnPropertyChanged();
                    ApplyTheme();
                }
            }
        }

        public ICommand ToggleThemeCommand { get; }

        public MainNavigation()
        {
            InitializeComponent();
            this.WindowState = WindowState.Maximized;

            var mainViewModel = new MainViewModel();
            DataContext = mainViewModel;

            // Optional: still use local theme handling if needed
            ToggleThemeCommand = new RelayCommand(() => mainViewModel.IsDarkMode = !mainViewModel.IsDarkMode);
            IsDarkMode = mainViewModel.IsDarkMode;
        }
        private void ToggleTheme()
        {
            IsDarkMode = !IsDarkMode;
        }
       
        private void ApplyTheme()
        {
            var dict = new ResourceDictionary
            {
                Source = new Uri(IsDarkMode
                    ? "Themes/DarkTheme.xaml"
                    : "Themes/LightTheme.xaml", UriKind.Relative)
            };

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void SetTheme(string themeName)
        {
            var dict = new ResourceDictionary();
            switch (themeName)
            {
                case "Dark":
                    dict.Source = new Uri("Themes/DarkTheme.xaml", UriKind.Relative);
                    break;
                case "Light":
                    dict.Source = new Uri("Themes/LightTheme.xaml", UriKind.Relative);
                    break;
            }

            Application.Current.Resources.MergedDictionaries.Clear();
            Application.Current.Resources.MergedDictionaries.Add(dict);
        }

        private void ThemeSelector_Changed(object sender, SelectionChangedEventArgs e)
        {
            var selected = (e.AddedItems[0] as ComboBoxItem)?.Content?.ToString();
            SetTheme(selected);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Placeholder
        }

        private void PlusButton_Click(object sender, RoutedEventArgs e)
        {
            AddWerkzeug addWerkzeug = new AddWerkzeug();
            Application.Current.MainWindow = addWerkzeug;
            addWerkzeug.Show();
            this.Close();
        }

        private void Projekt_Click(object sender, RoutedEventArgs e)
        {
            ProjektDetailsView projektDetailsView = new ProjektDetailsView();
            Application.Current.MainWindow = projektDetailsView;
            projektDetailsView.Show();
            this.Close();
        }

        private void Maschine_Lager(object sender, RoutedEventArgs e)
        {
            Lager lager = new Lager();
            Application.Current.MainWindow = lager;
            lager.Show();
            this.Close();
        }
        private void Logout_Click(object sender, RoutedEventArgs e)
        {
            var login = new LoginUser();
            Application.Current.MainWindow = login;
            login.Show();
            this.Close();

            MessageBox.Show("Logout gedrückt");

        }
        private void BackToProjekte_Click(object sender, RoutedEventArgs e)
        {
            ProjekteView projekteView = new ProjekteView();
            Application.Current.MainWindow = projekteView;
            projekteView.Show();
            this.Close(); 
        }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}