using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using TaskManager.DesktopClient.Views;

namespace TaskManager.DesktopClient.Resources.Commands.RegistrationWindowCommands
{
    public class ReturnToLoginWindowCommand : Abstractions.CommandBase
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            (parameter as Window).Hide();
            var loginWindow = new LoginWindow();
            loginWindow.Show();
        }
    }
}
