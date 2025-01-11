using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.DesktopClient.Services.ViewServices;
using TaskManager.Models.ClientModels;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.Resources.Commands.MainWindowCommands
{
    public class OpenProjectCommand : Commands.Abstractions.CommandBase
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {

        }
    }
}
/*
private void OpenNewProjectPage()
{
    _baseViewService.ShowMessage(nameof(OpenNewProjectPage));
}

private void UpdateProject(object parameter)
{
    var selectedProject = parameter as ClientModel<ProjectDto>;
    SelectedProject = selectedProject;
    _baseViewService.ShowMessage(nameof(ShowProjectInfo));
}
private void ShowProjectInfo(object parameter)
{
    var selectedProject = parameter as ClientModel<ProjectDto>;
    SelectedProject = selectedProject;
    _baseViewService.ShowMessage(nameof(ShowProjectInfo));
}
*/