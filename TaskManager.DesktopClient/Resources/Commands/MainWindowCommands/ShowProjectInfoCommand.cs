using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TaskManager.DesktopClient.Services.ViewServices;
using TaskManager.DesktopClient.Views.Pages;
using TaskManager.Models.ClientModels;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.Resources.Commands.MainWindowCommands
{
    public class ShowProjectInfoCommand : Commands.Abstractions.CommandBase
    {
        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter)
        {
            BaseViewService _baseViewService = new BaseViewService();

            var test = parameter as ClientModel<ProjectDto>; parameter.ToString();
            var test1 = test;// = parameter as ProjectsPage;
            //var test = window.projectsUpperItemControl.Items.CurrentItem as Models.ClientModels.ClientModel<ProjectDto>;// as List<Models.ClientModels.ClientModel<ProjectDto>>;
            //var test1 = test;
            //_baseViewService.ShowMessage(test.Model.Name + " " + test.Model.Description);
            //var ic = window.projectsUpperItemControl.
            //var scrollViewer = window.projectsScrollViewer.
        }
    }
}/*
private void ShowProjectInfo(object parameter)
{
    var selectedProject = parameter as ClientModel<ProjectDto>;
    SelectedProject = selectedProject;
    _baseViewService.ShowMessage(nameof(ShowProjectInfo));
}*/