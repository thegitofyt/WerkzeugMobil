using System;
using System.Windows.Input;

namespace ListDemo.ViewModels
{
    /// <summary>
    /// Generische Implementierung des ICommand Interfaces für das Command Binding.
    /// </summary>
    class RelayCommand : ICommand
    {
        private readonly Action<object?> _executeWithParam;
        private readonly Func<object?, bool>? _canExecuteWithParam;
        private readonly Action? _execute;
        private readonly Func<bool>? _canExecute;

        /// <summary>
        /// Constructor for commands that take a parameter.
        /// </summary>
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _executeWithParam = execute ?? throw new ArgumentNullException(nameof(execute), "Execute action cannot be null");
            _canExecuteWithParam = canExecute;
        }

        /// <summary>
        /// Constructor for parameterless commands.
        /// </summary>
        public RelayCommand(Action execute, Func<bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute), "Execute action cannot be null");
            _canExecute = canExecute;
        }

        public bool CanExecute(object? parameter)
        {
            if (_canExecuteWithParam != null) return _canExecuteWithParam(parameter);
            if (_canExecute != null) return _canExecute();
            return true; // Default to executable if no condition is provided
        }

        public void Execute(object? parameter)
        {
            if (_executeWithParam != null) _executeWithParam(parameter);
            else _execute?.Invoke();
        }

        public event EventHandler? CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }
    }
}
