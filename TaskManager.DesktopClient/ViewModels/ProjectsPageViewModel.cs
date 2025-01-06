using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using Prism.Commands;
using Prism.Mvvm;

using Prism.Mvvm;

using TaskManager.DesktopClient.Services;
using TaskManager.DesktopClient.Services.Abstractions;
using TaskManager.DesktopClient.Services.ViewServices;
using TaskManager.Models.ClientModels;
using TaskManager.Models.Content;
using TaskManager.Models.Dtos;

namespace TaskManager.DesktopClient.ViewModels
{
    public class ProjectsPageViewModel : BindableBase
    {
        private AuthToken _token;
        private TasksRequestService _tasksRequestService;
        private readonly UsersRequestService _usersRequestService = new UsersRequestService();
        private readonly ProjectsRequestService _projectsRequestsService = new ProjectsRequestService();

        private BaseViewService _baseViewService;
        //public List<ClientSideTaskModel> TasksCollection = new List<ClientSideTaskModel> { };

        #region COMMANDS 

        public DelegateCommand OpenNewProjectCommand;
        public DelegateCommand<object> OpenUpdateProjectCommand;
        //public DelegateCommand DeleteProjectCommand;
        //public DelegateCommand SaveProjectCommand;
        public DelegateCommand<object> ShowProjectInfoCommand;

        #endregion

        public ProjectsPageViewModel() { }
        public ProjectsPageViewModel(AuthToken token)
        {
            _baseViewService = new BaseViewService();
            _usersRequestService = new UsersRequestService();
            _projectsRequestsService = new ProjectsRequestService();
            _token = token;
            //_tasksRequestService = new TasksRequestService(_usersRequestService, _deskRequestsService);
            OnStartup(_token);
        }

        #region PROPERTIES

        private List<Models.ClientModels.ClientModel<ProjectDto>> _projectsCollection = new List<Models.ClientModels.ClientModel<ProjectDto>>();

        public List<Models.ClientModels.ClientModel<ProjectDto>> ProjectsCollection
        {
            get => _projectsCollection; 
            set 
            { 
                _projectsCollection = value; 
                RaisePropertyChanged(nameof(ProjectsCollection));
            }
        }

        private ClientModel<ProjectDto> _selectedProject;

        public ClientModel<ProjectDto> SelectedProject
        {
            get => _selectedProject;
            set
            {
                _selectedProject = value;
                RaisePropertyChanged(nameof(SelectedProject));
                if (SelectedProject.Model.UsersIds.Count > 0)
                {
                    GetProjectUsers(SelectedProject.Model.UsersIds);
                }
                
            }
        }

        private async Task<UserDto> GetUserDto(AuthToken token, Guid userId)
        {
            return await _usersRequestService.GetAsync(token, userId);
        }
        private async void GetProjectUsers(List<Guid> userIds)
        {
            List < UserDto > collection = new List<UserDto> ();

            foreach (var user in userIds) 
            {
                collection.Add(await GetUserDto(_token, user));
            }
            ProjectUsers = collection;
            //return collection;
        }
        //_usersRequestService.GetAsync(_token, userId) 

        private List<UserDto> _projectUsers = new List<UserDto>();

        public List<UserDto> ProjectUsers
        {
            get { return _projectUsers; }
            set { _projectUsers = value; RaisePropertyChanged(nameof(ProjectUsers)); }
        }




        #endregion


        #region OnStartup 

        private async void OnStartup(AuthToken token)
        {
            OpenNewProjectCommand = new DelegateCommand(OpenNewProject);
            OpenUpdateProjectCommand = new DelegateCommand<object>(OpenUpdateProject);
            ShowProjectInfoCommand = new DelegateCommand<object>(ShowProjectInfo);

            ProjectsCollection = await _projectsRequestsService.GetAllAsync(_token);
            //var test = ProjectsCollection.FirstOrDefault();
        }

        #endregion


        #region Methods 

        private void OpenNewProject()
        {
            _baseViewService.ShowMessage(nameof(OpenNewProject));
        }

        private void OpenUpdateProject(object parameter) 
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

        #endregion


    }
}
