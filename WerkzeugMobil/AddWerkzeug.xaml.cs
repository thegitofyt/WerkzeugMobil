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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WerkzeugMobil.MVVM.Viewmodel;

namespace WerkzeugMobil
{
    /// <summary>
    /// Interaction logic for Page1.xaml
    /// </summary>
    public partial class AddWerkzeug : Window
    {
        private AddProjekt addProjekt; // Declare AddProjekt as a class member to keep track of it
        private bool isDarkMode = false;
        public AddWerkzeug()
        {
            InitializeComponent();
            SetTheme(isDarkMode);
            DataContext = new AddWerkzeugViewModel();

            this.WindowState = WindowState.Maximized;

            addProjekt = new AddProjekt();  // Initialize the AddProjekt window
        }

       

        private void AnimateThemeChange()
        {
            var anim = new DoubleAnimation(0, 1, new Duration(TimeSpan.FromMilliseconds(300)));
            this.BeginAnimation(Window.OpacityProperty, anim);
        }

        private void ThemeToggle_Checked(object sender, RoutedEventArgs e)
        {
            isDarkMode = true;
            SetTheme(isDarkMode);
        }

        private void ThemeToggle_Unchecked(object sender, RoutedEventArgs e)
        {
            isDarkMode = false;
            SetTheme(isDarkMode);
        }

        private void SetTheme(bool darkMode)
        {
            if (darkMode)
            {
                // Dark mode settings
                Resources["AppBackground"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#00254D"));
                Resources["AppText"] = Brushes.White;  // White text for dark mode
                Resources["AppCard"] = new SolidColorBrush(Color.FromRgb(30, 30, 47));  // Dark card color
                Resources["AppBorder"] = new SolidColorBrush(Color.FromRgb(58, 58, 58));  // Dark border color
            }
            else
            {
                // Light mode settings
                Resources["AppBackground"] = new SolidColorBrush(Colors.White);
                Resources["AppText"] = Brushes.Black;  // Black text for light mode
                Resources["AppCard"] = Brushes.White;  // White card color
                Resources["AppBorder"] = new SolidColorBrush(Color.FromRgb(224, 224, 224));  // Light border color
            }
        }

        private void NavigateWerkzeug(object sender, RoutedEventArgs e)
        {
            // If the AddWerkzeug window is not open, show it
            if (!this.IsVisible)
            {
                this.Show(); // Show the AddWerkzeug window
            }

            // If the AddProjekt window is open, close it
            if (addProjekt.IsVisible)
            {
                addProjekt.Close();
            }
        }

        private void NavigateProjekt(object sender, RoutedEventArgs e)
        {
            // If the AddProjekt window is not open, show it
            if (!addProjekt.IsVisible)
            {
                addProjekt.Show(); // Show the AddProjekt window
            }

            // If the AddWerkzeug window is open, close it
            if (this.IsVisible)
            {
                this.Close();
            }
        }
    }
}