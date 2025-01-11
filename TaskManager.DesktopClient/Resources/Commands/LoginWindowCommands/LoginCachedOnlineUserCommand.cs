using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.DesktopClient.Views;

namespace TaskManager.DesktopClient.Resources.Commands.LoginWindowCommands
{
    class LoginCachedOnlineUserCommand : Resources.Commands.Abstractions.CommandBase
    {
        public override bool CanExecute(object parameter) => true;

        public async override void Execute(object parameter)
        {
            bool result = false;
            var loginService = new Services.ViewServices.LoginUserService();
            Views.Windows.LoadingImageWindow loadingInage = new Views.Windows.LoadingImageWindow();

            result = await loginService.LoginCachedUser(parameter);

            loadingInage.ShowDialog();

            if (result)
            {
                loadingInage.Close();
                loadingInage = null;
            }
        }
    }
}