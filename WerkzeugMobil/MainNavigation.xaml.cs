using System.Collections.Generic;
using System.Linq;
using WerkzeugMobil.MVVM.Model;

using System.Windows;
using WerkzeugMobil.MVVM.Viewmodel;

namespace WerkzeugMobil
{
    public partial class MainNavigation : Window
    {
        private readonly MainViewModel _viewModel;

        public MainNavigation(MainViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            WerkzeugeListView.ItemsSource = _viewModel.Werkzeuge;
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.WerkzeugeSuchen(SearchBar.Text);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var dataEntryWindow = new DataEntry(_viewModel);
            dataEntryWindow.ShowDialog();
        }
    }
}
