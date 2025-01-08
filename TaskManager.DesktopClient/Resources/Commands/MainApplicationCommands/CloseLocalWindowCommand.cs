using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManager.DesktopClient.Resources.Commands.MainApplicationCommands
{
    public class CloseLocalWindow : Commands.Abstractions.CommandBase
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            (parameter as Window).Hide();
        }
    }
}
