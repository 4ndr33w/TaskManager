using System;
using System.Windows;

using TaskManager.DesktopClient.Views;

namespace TaskManager.DesktopClient.Resources.Commands.LoginWindowCommands
{
    class LoginUserCommand : Commands.Abstractions.CommandBase
    {
        public override bool CanExecute(object parameter)
        {
            var loginWindow = parameter as LoginWindow;

            if (loginWindow != null)
            {
                bool isLoginEmpty = (loginWindow.LoginWindowLoginTextBox.Text == null || loginWindow.LoginWindowLoginTextBox.Text == String.Empty) ? true : false;
                bool isPasswordEmpty = (loginWindow.LoginWindowPasswordBox.Password == null || loginWindow.LoginWindowPasswordBox.Password == String.Empty) ? true : false;

                if (!isLoginEmpty && !isPasswordEmpty)
                {
                    return true;
                }

                else return false;
            }
            return false;
        }

        public override async void Execute(object parameter)
        {
            var loginService = new Services.ViewServices.LoginUserService();

            await loginService.LoginMethod(parameter);
        }
    }
}
