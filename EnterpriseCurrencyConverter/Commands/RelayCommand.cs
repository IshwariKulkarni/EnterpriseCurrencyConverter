using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace EnterpriseCurrencyConverter.Commands
{
    /// <summary>
    /// RelayCommand implements ICommand, which is what WPF Buttons bind to.
    /// This is the MOST important class in MVVM — memorize how it works!
    /// 
    /// ICommand has two members:
    ///   Execute(object)     → what happens when button is clicked
    ///   CanExecute(object)  → should the button be enabled?
    /// </summary>
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        // This event fires when CanExecute might have changed
        // WPF uses this to re-check if the button should be enabled/disabled
        public event EventHandler? CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        // Constructor: pass in what to do (execute) and optionally when to allow it (canExecute)
        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute = execute ?? throw new ArgumentNullException(nameof(execute));
            _canExecute = canExecute;
        }

        // WPF calls this to decide: should this button be clickable?
        public bool CanExecute(object? parameter)
        {
            return _canExecute == null || _canExecute(parameter);
        }

        // WPF calls this when the button is actually clicked
        public void Execute(object? parameter)
        {
            _execute(parameter);
        }
    }
}
