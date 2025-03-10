using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WerkzeugMobil.MVVM.Model;
using ListDemo.ViewModels;
using System.Windows;
namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class AddProjektViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public Projekt Projekt { get; set; }
        public ICommand SubmitCommand { get; }
        public ICommand NavigateWerkzeugCommand { get; }
        public AddProjektViewModel()
        {
            Projekt = new Projekt();
            SubmitCommand = new RelayCommand(Submit);
            NavigateWerkzeugCommand = new RelayCommand(NavigateToWerkzeug);
        }
        private void Submit() { /* Submit logic */ }
        private void NavigateToWerkzeug()
        {
            new AddWerkzeug().Show();
            Application.Current.Windows[0]?.Close();
        }
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
