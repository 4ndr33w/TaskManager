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
            var loginService = new Services.ViewServices.LoginUserService();

            await loginService.LoginCachedUser(parameter);
        }
    }
}
