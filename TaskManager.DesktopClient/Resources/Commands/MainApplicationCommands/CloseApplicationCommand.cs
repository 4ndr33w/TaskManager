using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace TaskManager.DesktopClient.Resources.Commands.MainApplicationCommands
{
    public class CloseApplicationCommand : Commands.Abstractions.CommandBase
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            Environment.Exit(0);
            //Application.Current.MainWindow.Close();
            //Application.Current.Shutdown();
        }
    }
}
