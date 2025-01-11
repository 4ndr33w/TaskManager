using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

using TaskManager.DesktopClient.Services;
using TaskManager.DesktopClient.Services.ViewServices;
using TaskManager.DesktopClient.ViewModels;
using TaskManager.DesktopClient.Views.Windows;
using TaskManager.Models.ClientModels;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.Resources.Commands.MainWindowCommands
{
    public class CreateNewProjectCommand : Commands.Abstractions.CommandBase
    {
        public override bool CanExecute(object parameter) => true;

        public override async void Execute(object parameter)
        {
            BaseViewService _baseViewService = new BaseViewService();
            ImageLoadSaveService _imageLoadSaveService = new ImageLoadSaveService();
            ProjectsRequestService _projectsRequestService = new ProjectsRequestService();

            //var projectModel = parameter as Models.ClientModels.ClientModel<ProjectDto>;
            //var newProjectDto = new ProjectDto();
            //var window = new CreateUpdateProjectWindow();

            //var dataContext = window.DataContext as ProjectsPageViewModel;


            //newProjectDto.Name = window.ProjectNameTextBox.Text;
            //newProjectDto.Description = window.ProjectDescriptionTextBox.Text;
            //byte[] picture = await _imageLoadSaveService.SearchImageFile();
            //newProjectDto.Image = picture;

            //var status = window.ProjectStatusComboBox.SelectedItem.ToString();



            //window.Hide();







            //var window = parameter as CreateUpdateProjectWindow;
            ////var dataContext = window.DataContext as ProjectsPageViewModel;

            //var newProjectDto = new ProjectDto();

            //newProjectDto.Name = window.

            //var selectedProject = dataContext.SelectedProject == null ? new ClientModel<ProjectDto>(new ProjectDto()) : dataContext.SelectedProject;

            //var createProjectWindow = new CreateUpdateProjectWindow(selectedProject);

            //createProjectWindow.DataContext = window.DataContext;

            //createProjectWindow.ShowDialog();
        }
    }
}
