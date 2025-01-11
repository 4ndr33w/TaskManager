using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.DesktopClient.Services.ViewServices;
using TaskManager.DesktopClient.Views.Windows;
using TaskManager.Models.ClientModels;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.Resources.Commands.Pages.ProjectPageCommands
{
    public class OpenEditProjectCommand : Commands.Abstractions.CommandBase
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            BaseViewService _baseViewService = new BaseViewService();

            var projectModel =  parameter as Models.ClientModels.ClientModel<ProjectDto>;
            var editProjectWindow = new CreateUpdateProjectWindow(projectModel);

            //_baseViewService.
            

        }
    }
}
