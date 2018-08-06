using System;
using System.Windows.Input;

namespace FiscalCode.Commands
{
    public interface IRelayCommand : ICommand
    {
        event EventHandler<CommandEventArgs> Executed;
    }

    public interface IRelayCommand<T> : ICommand
    {
        event EventHandler<CommandEventArgs<T>> Executed;
    }
}
