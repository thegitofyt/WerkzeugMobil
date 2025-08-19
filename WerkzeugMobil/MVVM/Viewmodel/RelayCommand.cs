using System;
using System.Windows.Input;

namespace WerkzeugMobil.MVVM.Viewmodel
{
    /// <summary>
    /// Generische Implementierung des ICommand Interfaces für das Command Binding.
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _executeWithParam;
        private readonly Func<object?, bool>? _canExecuteWithParam;
        private readonly Action? _execute;
        private readonly Func<bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _executeWithParam = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecuteWithParam = canExecute;
        }

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecuteWithParam != null) return _canExecuteWithParam(parameter);
            if (_canExecute != null) return _canExecute();
            return true;
        }

        public void Execute(object? parameter)
        {
            if (_executeWithParam != null) _executeWithParam(parameter);
            else _execute?.Invoke();
        }

        public event EventHandler? CanExecuteChanged;

        public void RaiseCanExecuteChanged()
        {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
