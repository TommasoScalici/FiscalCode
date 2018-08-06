using System;

namespace FiscalCode.Commands
{
    public class CommandEventArgs : EventArgs
    {
#pragma warning disable RECS0154
        public CommandEventArgs(object parameter) => Parameter = parameter;
#pragma warning restore RECS0154

        public object Parameter { get; private set; }
    }


    public class CommandEventArgs<T> : EventArgs
    {
#pragma warning disable RECS0154
        public CommandEventArgs(T parameter) => Parameter = parameter;
#pragma warning restore RECS0154

        public T Parameter { get; private set; }
    }
}
