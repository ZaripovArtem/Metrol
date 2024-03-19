using System.Windows.Input;

namespace Metrol.ViewModel
{
    class RelayCommand : ICommand
    {
        private readonly Action<object> _execute;
        private readonly Predicate<object> _canExecute;

        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
            {
                throw new ArgumentNullException("execute");
            }

            if (canExecute == null)
            {
                throw new ArgumentNullException("canExecute");
            }

            _execute = execute;
            _canExecute = canExecute;
        }

        public event EventHandler? CanExecuteChanged
        {
            add 
            { 
                CommandManager.RequerySuggested += value; 
            }
            remove 
            { 
                CommandManager.RequerySuggested -= value; 
            }
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute == null 
                ? true 
                : _canExecute(parameter);
        }

        public void Execute(object parameter)
        {
            _execute(parameter);
        }

    }
}
