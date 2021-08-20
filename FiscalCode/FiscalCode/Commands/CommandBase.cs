using System;
using System.Windows.Input;
using PropertyChanged;

namespace FiscalCode.Commands
{
    [AddINotifyPropertyChangedInterface]
    
    public abstract class CommandBase : ICommand
    {
        public event EventHandler CanExecuteChanged;


        public abstract bool CanExecute(object parameter);
        public abstract void Execute(object parameter, bool ignoreCanExecute);
        public void Execute(object parameter = null) => Execute(parameter, false);
        public void RaiseCanExecuteChanged() => OnCanExecuteChanged();
        [SuppressPropertyChangedWarnings]
        protected virtual void OnCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        protected virtual void OnExecuted(EventArgs e) => RaiseCanExecuteChanged();
    }
}
