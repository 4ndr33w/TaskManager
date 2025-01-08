using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.DesktopClient.Resources.Commands.Abstractions
{
    public abstract class RelayCommand : CommandBase
    {
        readonly Action<object> _execute;
        readonly Func<object, bool> _canExecute;

        public RelayCommand(Action<object> execute, Func<object, bool> canExecute)
        {
            _execute = execute;
            _canExecute = canExecute;
        }
        public RelayCommand(Action<object> execute)
        {
            _execute = execute;
        }

        public override bool CanExecute(object parameter)
        {
            if (_canExecute != null) return _canExecute(parameter);
            return true;
        }

        public override void Execute(object parameter)
        {
            if (_execute != null) _execute(parameter);
        }
    }
}
