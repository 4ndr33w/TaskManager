using System;
using System.Threading.Tasks;
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
            bool result = false;
            var loginService = new Services.ViewServices.LoginUserService();
            Views.Windows.LoadingImageWindow loadingInage = new Views.Windows.LoadingImageWindow();

            try
            {
                result = await loginService.LoginMethod(parameter);
                loadingInage.ShowDialog();

                if (result)
                {
                    loadingInage.Close();
                    loadingInage.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                loadingInage.Close();
                loadingInage.Visibility = Visibility.Collapsed;
            }

            loadingInage.Close();
            loadingInage.Visibility = Visibility.Collapsed;
        }
    }
}
