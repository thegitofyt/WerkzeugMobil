using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WerkzeugMobil.DTO;
using WerkzeugMobil.MVVM.Model;
using WerkzeugMobil.Services;
using System.Windows;
using System.Collections.ObjectModel;
using WerkzeugMobil.Data;
using System.Text.Json;
using ListDemo.ViewModels;

namespace WerkzeugMobil.MVVM.Viewmodel
{
    public class AddWerkzeugViewModel : INotifyPropertyChanged
    {
        private readonly WerkzeugServices _werkzeugService;

        public event PropertyChangedEventHandler? PropertyChanged;

        private WerkzeugDto _selectedWerkzeug;
        private Werkzeug _werkzeug;
        private ObservableCollection<ToolDTO> _tools;
        private Tools _selectedTool;

        public ICommand AddNewCommand { get; }
        public ICommand SubmitCommand { get; }

        public Werkzeug Werkzeug
        {
            get => _werkzeug;
            set => SetProperty(ref _werkzeug, value);
        }

        public WerkzeugDto SelectedWerkzeug
        {
            get => _selectedWerkzeug;
            private set => SetProperty(ref _selectedWerkzeug, value);
        }

        public ObservableCollection<ToolDTO> Tools
        {
            get => _tools;
            set => SetProperty(ref _tools, value);
        }

        public Tools SelectedTool
        {
            get => _selectedTool;
            set => SetProperty(ref _selectedTool, value);
        }

        // Default constructor for new Werkzeug creation
        public AddWerkzeugViewModel()
        {
            _werkzeugService = new WerkzeugServices();
            Werkzeug = new Werkzeug();
            AddNewCommand = new RelayCommand(AddNew);
            SubmitCommand = new RelayCommand(Submit);
            LoadTools();
        }

        // Overloaded constructor for editing an existing Werkzeug
        public AddWerkzeugViewModel(WerkzeugDto existingWerkzeug) : this()
        {
            if (existingWerkzeug != null)
            {
                SelectedWerkzeug = existingWerkzeug;
                Werkzeug = new Werkzeug
                {
                    WerkzeugId = existingWerkzeug.WerkzeugId,
                    Marke = existingWerkzeug.Marke,
                    Art = existingWerkzeug.Art,
                    ProjektAdresse = existingWerkzeug.ProjektAdresse,
                    Beschreibung = existingWerkzeug.Beschreibung
                };
            }
        }

        private void LoadTools()
        {
            try
            {
                using (var context = new WerkzeugDbContext())
                {
                    var tools = context.Tools.ToList();
                    Tools = new ObservableCollection<ToolDTO>(
                        tools.Select(t => new ToolDTO
                        {
                            Id = t.Id,
                            Name = t.Name,
                            ToolTypeCounts = JsonSerializer.Deserialize<List<Tuple<string, int>>>(t.ToolTypeCountsSerialized)
                        })
                    );
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading Tools: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddNew()
        {
            Werkzeug = new Werkzeug(); // Reset for new input
            SelectedWerkzeug = null;
        }

        private void Submit()
        {
            try
            {
                if (SelectedTool == null || string.IsNullOrEmpty(SelectedTool.Name))
                {
                    MessageBox.Show("Bitte ein Werkzeug auswählen.", "Fehler", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                Werkzeug.Art = SelectedTool.Name;

                var dtoToSave = new WerkzeugDto
                {
                    WerkzeugId = Werkzeug.WerkzeugId,
                    Marke = Werkzeug.Marke,
                    Art = Werkzeug.Art,
                    ProjektAdresse = Werkzeug.ProjektAdresse,
                    Beschreibung = Werkzeug.Beschreibung
                };

                _werkzeugService.AddWerkzeug(dtoToSave);
                _werkzeugService.UpdateAddressHistory(dtoToSave.WerkzeugId, dtoToSave.ProjektAdresse);

                MessageBox.Show("Werkzeug erfolgreich hinzugefügt!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
                AddNew();
            }
            catch (Exception ex)
            {
                string errorMessage = $"Fehler beim Hinzufügen des Werkzeugs: {ex.Message}";
                if (ex.InnerException != null)
                    errorMessage += $"\nInner Exception: {ex.InnerException.Message}";

                MessageBox.Show(errorMessage, "Fehler", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private RelayCommand navigateWerkzeugCommand;
        public ICommand NavigateWerkzeugCommand => navigateWerkzeugCommand ??= new RelayCommand(NavigateWerkzeug);

        private void NavigateWerkzeug(object commandParameter) { }

        private RelayCommand navigateProjektCommand;
        public ICommand NavigateProjektCommand => navigateProjektCommand ??= new RelayCommand(NavigateProjekt);

        private void NavigateProjekt(object commandParameter) { }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }
    }
}