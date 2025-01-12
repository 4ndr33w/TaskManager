using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using TaskManager.DesktopClient.Views;

namespace TaskManager.DesktopClient.Resources.Commands.LoginWindowCommands
{
    class OpenRegistrationWindowCommand : Commands.Abstractions.CommandBase
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            Views.Windows.RegistrationWindow registrationWindow = new Views.Windows.RegistrationWindow();
            registrationWindow.Owner = Application.Current.MainWindow;
            registrationWindow.ShowDialog();
        }
    }
}
