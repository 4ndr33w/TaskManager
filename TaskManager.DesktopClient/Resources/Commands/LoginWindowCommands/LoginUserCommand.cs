using System;
using System.Threading.Tasks;
using System.Windows;

using TaskManager.DesktopClient.Views;

namespace TaskManager.DesktopClient.Resources.Commands.LoginWindowCommands
{
    class LoginUserCommand : Commands.Abstractions.CommandBase
    {
        public override bool CanExecute(object parameter)// => true;
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
            Views.Windows.LoadingImageWindow loadingImage = new Views.Windows.LoadingImageWindow();

            try
            {
                result = await loginService.LoginMethod(parameter);
                loadingImage.ShowDialog();

                if (result)
                {
                    loadingImage.EndInit();
                    loadingImage.Close();
                    loadingImage.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception)
            {
                loadingImage.EndInit();
                loadingImage.Close();
                loadingImage.Visibility = Visibility.Collapsed;
            }

            loadingImage.EndInit();
            loadingImage.Close();
            loadingImage.Visibility = Visibility.Collapsed;
            loadingImage.EndInit();
        }
    }
}
