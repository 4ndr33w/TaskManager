using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

using Prism.Commands;

using Prism.Mvvm;
using Prism.Mvvm;

using TaskManager.DesktopClient.Services;
using TaskManager.DesktopClient.Services.Abstractions;
using TaskManager.DesktopClient.Services.ViewServices;
using TaskManager.DesktopClient.Views.Windows;
using TaskManager.Models.ClientModels;
using TaskManager.Models.Content;
using TaskManager.Models.Dtos;
using TaskManager.Models.ShortInfoModels;

namespace TaskManager.DesktopClient.ViewModels
{
    public class ProjectsPageViewModel : BindableBase
    {
        private AuthToken _token;
        private TasksRequestService _tasksRequestService;
        private readonly UsersRequestService _usersRequestService = new UsersRequestService();
        private readonly ProjectsRequestService _projectsRequestsService = new ProjectsRequestService();

        private MainWindowViewModel _mainWindowViewModel {  set; get; }

        private BaseViewService _baseViewService;

        private byte[] _picture;
        public byte[] Picture
        {
            get => _picture;
            set
            {
                _picture = value;
                RaisePropertyChanged(nameof(Picture));
            }
        }
        private Page _selectedPage;
        public Page SelectedPage
        {
            get => _selectedPage;
            private set
            {
                _selectedPage = value;
                RaisePropertyChanged(nameof(SelectedPage));
            }
        }

        private Page _projectPage;
        public Page ProjectPage
        {
            get => _projectPage;
            private set
            {
                _projectPage = value;
                RaisePropertyChanged(nameof(ProjectPage));
            }
        }

        private Page _createUpdateprojectPage;
        public Page CreateUpdateprojectPage
        {
            get => _createUpdateprojectPage;
            private set
            {
                _createUpdateprojectPage = value;
                RaisePropertyChanged(nameof(CreateUpdateprojectPage));
            }
        }

        private ObservableCollection<UserInfo> _usersInfoCollection;
        public ObservableCollection<UserInfo> UsersInfoCollection
        {
            get => _usersInfoCollection;
            set
            {
                _usersInfoCollection = value;
                RaisePropertyChanged(nameof(UsersInfoCollection));
            }
        }




        #region COMMANDS 

        public DelegateCommand OpenNewProjectPageCommand { get; private set; }
        public DelegateCommand<object> OpenUpdateProjectCommand { get; private set; }
        public DelegateCommand<object> CreateNewProjectCommand { get; private set; }
        //public DelegateCommand DeleteProjectCommand;
        //public DelegateCommand SaveProjectCommand;
        public DelegateCommand<object> ShowProjectInfoCommand { get; private set; }

        public DelegateCommand OpenProjectsPageCommand { get; private set; }
        public DelegateCommand AbortCreatingPageCommand { get; private set; }

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

        public ProjectsPageViewModel(AuthToken token, Page selectedPage)
        {
            _baseViewService = new BaseViewService();
            _usersRequestService = new UsersRequestService();
            _projectsRequestsService = new ProjectsRequestService();
            _token = token;
            
            ProjectPage = new Views.Pages.ProjectsPage();
            ProjectPage = selectedPage;
            //_tasksRequestService = new TasksRequestService(_usersRequestService, _deskRequestsService);
            SelectedPage = selectedPage;
            CreateUpdateprojectPage = new Views.Pages.CreateProjectPage();
            OnStartup(_token);
        }
        public ProjectsPageViewModel(AuthToken token, Page selectedPage, MainWindowViewModel mainWindowVM)
        {
            _baseViewService = new BaseViewService();
            _usersRequestService = new UsersRequestService();
            _projectsRequestsService = new ProjectsRequestService();
            _token = token;

            ProjectPage = new Views.Pages.ProjectsPage();
            ProjectPage = selectedPage;
            //_tasksRequestService = new TasksRequestService(_usersRequestService, _deskRequestsService);
            SelectedPage = selectedPage;
            
            _mainWindowViewModel = mainWindowVM;
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
                GetProjectUsers(SelectedProject.Model.UsersIds);
            }
        }

        private ClientModel<ProjectDto> _newProject;

        public ClientModel<ProjectDto> NewProject
        {
            get => _newProject;
            set
            {
                _newProject = value;
                RaisePropertyChanged(nameof(NewProject));
                //GetProjectUsers(SelectedProject.Model.UsersIds);
            }
        }


        private async Task<UserDto> GetUserDto(AuthToken token, Guid userId)
        {
            return await _usersRequestService.GetAsync(token, userId);
        }
        private async void GetProjectUsers(List<Guid> userIds)
        {
            if (userIds != null && userIds.Count > 0)
            {
                List<UserDto> collection = new List<UserDto>();

                foreach (var user in userIds)
                {
                    collection.Add(await GetUserDto(_token, user));
                }
                ProjectUsers = collection;
            }
            else ProjectUsers = new List<UserDto>();
        }

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
            OpenNewProjectPageCommand = new DelegateCommand(OpenNewProjectPage);
            OpenUpdateProjectCommand = new DelegateCommand<object>(OpenUpdateProject);
            ShowProjectInfoCommand = new DelegateCommand<object>(ShowProjectInfo);

            ProjectsCollection = await _projectsRequestsService.GetAllAsync(_token);
            CreateNewProjectCommand = new DelegateCommand<object>(CreateNewProject);
            OpenProjectsPageCommand = new DelegateCommand(OpenProjectsPage);
            AbortCreatingPageCommand = new DelegateCommand(AbortCreatingPage);

            //CreateUpdateprojectPage = new Views.Pages.CreateProjectPage();

            //CreateUpdateprojectPage
            //var test = ProjectsCollection.FirstOrDefault();
        }

        #endregion


        #region Methods 

        private void AbortCreatingPage()
        {
            _mainWindowViewModel.OpenPage(ProjectPage, Resources.TextData.UserProjectsButtonName, this);
        }

        private async void OpenNewProjectPage()
        {
            CreateUpdateprojectPage = new Views.Pages.CreateProjectPage();
            UsersInfoCollection = new ObservableCollection<UserInfo>();
            UsersInfoCollection.Clear();

            var usersInfoList = await _usersRequestService.GetUsersInfoAsync(_token);

            foreach (var item in usersInfoList)
            {
                UsersInfoCollection.Add(item);
            }
            _mainWindowViewModel.OpenPage(CreateUpdateprojectPage, Resources.TextData.CreateNewProjectString, this);
        }

        private void OpenProjectsPage()
        {
            SelectedPage = ProjectPage;
        }

        private async void CreateNewProject(object parameter)
        {
            var projectModel = parameter as Models.ClientModels.ClientModel<ProjectDto>;
            var newProjectDto = new ProjectDto();

            var window = parameter as CreateUpdateProjectWindow;
            newProjectDto.Name = window.ProjectNameTextBox.Text;
            newProjectDto.Description = window.ProjectDescriptionTextBox.Text;
            newProjectDto.Image = Picture;

            var status = window.ProjectStatusComboBox.SelectedItem.ToString();

            newProjectDto.ProjectStatus = DefineProjectStatus(status);

            var result = await _projectsRequestsService.CreateAsync(newProjectDto, _token);

            _baseViewService.ShowMessage(result);
            if (result.ToLower() == "true")
            {
                _baseViewService.ShowMessage("Project is cucseed created");
            }
            else
            {
                _baseViewService.ShowMessage("Error during saving project");
            }
            window.Hide();
        }

        private void OpenUpdateProject(object parameter) 
        {
            if (parameter != null)
            {
                var selectedProject = parameter as ClientModel<ProjectDto>;
                SelectedProject = selectedProject;
                var editProjectWindow = new CreateUpdateProjectWindow(selectedProject);

                editProjectWindow.DataContext = this;
                editProjectWindow.ShowDialog();
            }
            else
            {
                MessageBox.Show("Empty");
            }
        }

        private void ShowProjectInfo(object parameter)
        {
            var selectedProject = parameter as ClientModel<ProjectDto>;
            SelectedProject = selectedProject;
        }

        private Models.Enums.ProjectStatus DefineProjectStatus(string status)
        {
            if (status == Models.Enums.ProjectStatus.InProgress.ToString())
            {
                return Models.Enums.ProjectStatus.InProgress;
            }
            if (status == Models.Enums.ProjectStatus.Expired.ToString())
            {
                return Models.Enums.ProjectStatus.Expired;
            }
            if (status == Models.Enums.ProjectStatus.Completed.ToString())
            {
                return Models.Enums.ProjectStatus.Completed;
            }
            if (status == Models.Enums.ProjectStatus.Failed.ToString())
            {
                return Models.Enums.ProjectStatus.Failed;
            }
            if (status == Models.Enums.ProjectStatus.Suspended.ToString())
            {
                return Models.Enums.ProjectStatus.Suspended;
            }

            return Models.Enums.ProjectStatus.InProgress;
        }

        #endregion
    }
}
