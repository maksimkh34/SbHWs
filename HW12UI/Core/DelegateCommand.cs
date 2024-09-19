using System.Windows.Input;

namespace HW12UI.Core
{
    public class DelegateCommand(Action<object> openAction, Predicate<object>?
        canExecutePredicate = null, Action? closeWindow = null) : ICommand
    {
        public static bool DefaultCanExecute(object param) => true;

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return (canExecutePredicate ?? DefaultCanExecute)(parameter!);
        }

        public void Execute(object? parameter)
        {
            openAction(parameter!);
            try { closeWindow?.Invoke(); }
            catch (System.ComponentModel.Win32Exception) { }
        }

        public void CheckCanExecute() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
    }
}